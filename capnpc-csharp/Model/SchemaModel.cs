﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CapnpC.Model
{
    class SchemaModel
    {
        readonly Schema.CodeGeneratorRequest.Reader _request;
        readonly List<GenFile> _generatedFiles = new List<GenFile>();
        Dictionary<ulong, IHasNestedDefinitions> _allDefinitions = new Dictionary<ulong, IHasNestedDefinitions>();
        readonly TypeDefinitionManager _typeDefMgr = new TypeDefinitionManager();

        readonly Dictionary<ulong, Schema.Node.Reader> _id2node = new Dictionary<ulong, Schema.Node.Reader>();

        public SchemaModel(Schema.CodeGeneratorRequest.Reader request)
        {
            _request = request;
        }

        public IReadOnlyList<GenFile> FilesToGenerate => _generatedFiles;

        Schema.Node.Reader? IdToNode(ulong id, bool mustExist)
        {
            if (_id2node.TryGetValue(id, out var node))
                return node;
            if (mustExist)
                throw new InvalidSchemaException($"Node with ID {id.StrId()} is required by the codegen backend but is missing.");
            return null;
        }

        Schema.Node.Reader IdToNode(ulong id)
        {
            return (Schema.Node.Reader)IdToNode(id, true);
        }

        void Build()
        {
            if (_request.Nodes == null || _request.Nodes.Count == 0)
            {
                throw new InvalidSchemaException("No nodes, nothing to generate");
            }

            foreach (var node in _request.Nodes)
            {
                if (_id2node.TryGetValue(node.Id, out var existingNode))
                {
                    throw new InvalidSchemaException($"Node {node.StrId()} \"{node.DisplayName}\" has a duplicate ID, prior node was \"{existingNode.DisplayName}\"");
                }
                _id2node[node.Id] = node;
            }

            BuildPass1();
            BuildPass2();
        }

        // First pass: create type definitions for each node.

        struct Pass1State
        {
            public HashSet<ulong> unprocessedNodes;
            public bool isGenerated;
            public IHasNestedDefinitions parent;
        }

        void BuildPass1()
        {
            Pass1State state = new Pass1State()
            {
                unprocessedNodes = new HashSet<ulong>(_id2node.Keys)
            };
            var requestedFiles = _request.RequestedFiles.ToDictionary(req => req.Id);
            foreach (var node in _id2node.Values.Where(n => n.IsFile))
            {
                GenFile file;
                state.isGenerated = requestedFiles.TryGetValue(node.Id, out var req);
                state.parent = null;
                if (state.isGenerated)
                {
                    file = (GenFile)ProcessNodePass1(node.Id, req.Filename, state);
                    _generatedFiles.Add(file);
                }
                else
                {
                    file = (GenFile)ProcessNodePass1(node.Id, node.DisplayName, state);
                }
                _allDefinitions.Add(node.Id, file);
            }
            if (state.unprocessedNodes.Count != 0)
            {
                throw new InvalidSchemaException("Unreferenced nodes were present in the schema.");
            }
        }

        TypeDefinition CreateTypeDef(Schema.Node.Reader node, IHasNestedDefinitions parent)
        {
            var kind = node.GetKind();
            var def = _typeDefMgr.Create(node.Id, kind.GetTypeTag());
            def.DeclaringElement = parent;
            if (kind == NodeKind.Group)
                ((TypeDefinition)parent).NestedGroups.Add(def);
            else
                parent.NestedTypes.Add(def);
            return def;
        }

        IHasNestedDefinitions ProcessNodePass1(ulong id, string name, Pass1State state)
        {
            if (!_id2node.TryGetValue(id, out var node))
            {
                if (!state.isGenerated) return null;
                throw new InvalidSchemaException($"The node {id.StrId()} was necessary for backend codegen but is missing.");
            }

            if (!state.unprocessedNodes.Remove(id))
                throw new InvalidSchemaException($"The node \"{node.DisplayName}\" {node.StrId()} has been declared recursively.");

            GenFile file = null;
            bool processNestedNodes = false;
            bool processFields = false;
            bool processInterfaceMethods = false;

            switch (node.GetKind())
            {
                case NodeKind.Annotation:
                case NodeKind.Const:
                    // A dummy TypeDefinition is created to node hierarchy
                    break;
                case NodeKind.File:
                    if (state.parent != null)
                        throw new InvalidSchemaException("Did not expect file nodes to appear as nested nodes");
                    file = new GenFile();
                    file.Namespace = GetNamespaceAnnotation(node);
                    processNestedNodes = true;
                    break;
                case NodeKind.Enum:
                    break;
                case NodeKind.Interface:
                    processNestedNodes = true;
                    processFields = true;
                    processInterfaceMethods = true;
                    break;
                case NodeKind.Struct:
                case NodeKind.Group:
                    processNestedNodes = true;
                    processFields = true;
                    break;
                default:
                    throw new InvalidSchemaException($"Don't know how to process node {node.StrId()} \"{node.DisplayName}\"");
            }

            TypeDefinition def = null;
            if (file != null)
            {
                state.parent = file;
                file.Name = name;
            }
            else
            {
                state.parent = def = CreateTypeDef(node, state.parent);
                def.Name = name;
            }
            
            if (processNestedNodes && node.NestedNodes != null)
                foreach (var nested in node.NestedNodes)
                {
                    ProcessNodePass1(nested.Id, nested.Name, state);
                }
            if (processFields && node.Fields != null)
                foreach (var field in node.Fields.Where(f => f.IsGroup))
                {
                    var group = IdToNode(field.Group_TypeId);
                    if (!group.IsStruct || !group.Struct_IsGroup)
                    {
                        throw new InvalidSchemaException($"Expected node with id {group.StrId()} to be a struct definition");
                    }
                    ProcessNodePass1(field.Group_TypeId, field.Name, state);
                }
            if (processInterfaceMethods && node.Interface_Methods != null)
                foreach (var method in node.Interface_Methods)
                {
                    var pnode = IdToNode(method.ParamStructType);
                    if (pnode.ScopeId == 0) ProcessNodePass1(pnode.Id, null, state); // Anonymous generated type
                    pnode = IdToNode(method.ResultStructType);
                    if (pnode.ScopeId == 0) ProcessNodePass1(pnode.Id, null, state); // Anonymous generated type
                }
            return state.parent;
        }

        string[] GetNamespaceAnnotation(Schema.Node.Reader fileNode)
        {
            foreach (var annotation in fileNode.Annotations)
            {
                if (annotation.Id == 0xb9c6f99ebf805f2c) // Cxx namespace
                {
                    return annotation.Value.Text.Split(new string[1] { "::" }, default);
                }
            }
            return null;
        }

        // 2nd pass: Generate types based on definitions

        struct Pass2State
        {
            public Method currentMethod;
            public bool isGenerated;
            public HashSet<ulong> processedNodes;
        }

        void BuildPass2()
        {
            var files = _allDefinitions.Select(d => (Id: d.Key, File: d.Value as GenFile)).Where(d => d.File != null);
            var state = new Pass2State() { processedNodes = new HashSet<ulong>() };
            foreach (var file in files)
            {
                var node = IdToNode(file.Id);
                state.isGenerated = _request.RequestedFiles.Where(req => req.Id == file.Id).Any();
                ProcessNestedNodes(node.NestedNodes, state);
            }
        }

        void ProcessNestedNodes(IEnumerable<Schema.Node.NestedNode.Reader> nestedNodes, Pass2State state)
        {
            foreach (var nestedNode in nestedNodes)
            {
                ProcessNode(nestedNode.Id, state);
            }
        }

        void ProcessBrand(Schema.Brand.Reader brandReader, Type type, Pass2State state)
        {
            foreach (var scopeReader in brandReader.Scopes)
            {
                var whatToBind = ProcessNode(scopeReader.ScopeId, state);
                int index = 0;

                switch (0)
                {
                    case 0 when scopeReader.IsBind:
                        foreach (var bindingReader in scopeReader.Bind)
                        {
                            var typeParameter = new GenericParameter()
                            {
                                DeclaringEntity = whatToBind,
                                Index = index++
                            };

                            switch (0)
                            {
                                case 0 when bindingReader.IsType:
                                    type.BindGenericParameter(typeParameter, ProcessType(bindingReader.Type, state));
                                    break;

                                case 0 when bindingReader.IsUnbound:
                                    type.BindGenericParameter(typeParameter, Types.FromParameter(typeParameter));
                                    break;
                            }
                        }
                        break;

                    case 0 when scopeReader.IsInherit:
                        for (index = 0; index < type.DeclaringType.Definition.GenericParameters.Count; index++)
                        {
                            var typeParameter = new GenericParameter()
                            {
                                DeclaringEntity = whatToBind,
                                Index = index
                            };

                            type.BindGenericParameter(typeParameter, Types.FromParameter(typeParameter));
                        }
                        break;
                }
            }
        }

        Type ProcessType(Schema.Type.Reader typeReader, Pass2State state)
        {
            Type result;

            switch (0)
            {
                case 0 when typeReader.IsAnyPointer:
                    switch (0)
                    {
                        case 0 when typeReader.AnyPointer_IsParameter:
                            return Types.FromParameter(
                                new GenericParameter()
                                {
                                    DeclaringEntity = ProcessNode(typeReader.AnyPointer_Parameter_ScopeId, state),
                                    Index = typeReader.AnyPointer_Parameter_ParameterIndex
                                });

                        case 0 when typeReader.AnyPointer_IsImplicitMethodParameter:
                            return Types.FromParameter(
                                new GenericParameter()
                                {
                                    DeclaringEntity = state.currentMethod ?? throw new InvalidOperationException("current method not set"),
                                    Index = typeReader.AnyPointer_ImplicitMethodParameter_ParameterIndex
                                });

                        case 0 when typeReader.AnyPointer_IsUnconstrained:

                            switch (0)
                            {
                                case 0 when typeReader.AnyPointer_Unconstrained_IsAnyKind:
                                    return Types.AnyPointer;

                                case 0 when typeReader.AnyPointer_Unconstrained_IsCapability:
                                    return Types.CapabilityPointer;

                                case 0 when typeReader.AnyPointer_Unconstrained_IsList:
                                    return Types.ListPointer;

                                case 0 when typeReader.AnyPointer_Unconstrained_IsStruct:
                                    return Types.StructPointer;

                                default:
                                    throw new NotImplementedException();
                            }

                        default:
                            throw new NotImplementedException();
                    }

                case 0 when typeReader.IsBool:
                    return Types.Bool;

                case 0 when typeReader.IsData:
                    return Types.Data;

                case 0 when typeReader.IsFloat64:
                    return Types.F64;

                case 0 when typeReader.IsEnum:
                    return Types.FromDefinition(ProcessNode(typeReader.Enum_TypeId, state, TypeTag.Enum));

                case 0 when typeReader.IsFloat32:
                    return Types.F32;

                case 0 when typeReader.IsInt16:
                    return Types.S16;

                case 0 when typeReader.IsInt32:
                    return Types.S32;

                case 0 when typeReader.IsInt64:
                    return Types.S64;

                case 0 when typeReader.IsInt8:
                    return Types.S8;

                case 0 when typeReader.IsInterface:
                    result = Types.FromDefinition(ProcessNode(typeReader.Interface_TypeId, state, TypeTag.Interface));
                    ProcessBrand(typeReader.Interface_Brand, result, state);
                    return result;

                case 0 when typeReader.IsList:
                    return Types.List(ProcessType(typeReader.List_ElementType, state));

                case 0 when typeReader.IsStruct:
                    result = Types.FromDefinition(ProcessNode(typeReader.Struct_TypeId, state, TypeTag.Struct));
                    ProcessBrand(typeReader.Struct_Brand, result, state);
                    return result;

                case 0 when typeReader.IsText:
                    return Types.Text;

                case 0 when typeReader.IsUInt16:
                    return Types.U16;

                case 0 when typeReader.IsUInt32:
                    return Types.U32;

                case 0 when typeReader.IsUInt64:
                    return Types.U64;

                case 0 when typeReader.IsUInt8:
                    return Types.U8;

                case 0 when typeReader.IsVoid:
                    return Types.Void;

                default:
                    throw new NotImplementedException();
            }
        }

        Value ProcessValue(Schema.Value.Reader valueReader)
        {
            var value = new Value();

            switch (0)
            {
                case 0 when valueReader.IsAnyPointer:
                    value.ScalarValue = valueReader.AnyPointer;
                    value.Type = Types.AnyPointer;
                    break;

                case 0 when valueReader.IsBool:
                    value.ScalarValue = valueReader.Bool;
                    value.Type = Types.Bool;
                    break;

                case 0 when valueReader.IsData:
                    value.Items.AddRange(valueReader.Data.CastByte().Select(Value.Scalar));
                    value.Type = Types.Data;
                    break;

                case 0 when valueReader.IsEnum:
                    value.ScalarValue = valueReader.Enum;
                    value.Type = Types.AnyEnum;
                    break;

                case 0 when valueReader.IsFloat32:
                    value.ScalarValue = valueReader.Float32;
                    value.Type = Types.F32;
                    break;

                case 0 when valueReader.IsFloat64:
                    value.ScalarValue = valueReader.Float64;
                    value.Type = Types.F64;
                    break;

                case 0 when valueReader.IsInt16:
                    value.ScalarValue = valueReader.Int16;
                    value.Type = Types.S16;
                    break;

                case 0 when valueReader.IsInt32:
                    value.ScalarValue = valueReader.Int32;
                    value.Type = Types.S32;
                    break;

                case 0 when valueReader.IsInt64:
                    value.ScalarValue = valueReader.Int64;
                    value.Type = Types.S64;
                    break;

                case 0 when valueReader.IsInt8:
                    value.ScalarValue = valueReader.Int8;
                    value.Type = Types.S8;
                    break;

                case 0 when valueReader.IsInterface:
                    value.ScalarValue = null;
                    value.Type = Types.CapabilityPointer;
                    break;

                case 0 when valueReader.IsList:
                    value.RawValue = valueReader.List;
                    value.Type = Types.ListPointer;
                    break;

                case 0 when valueReader.IsStruct:
                    value.RawValue = valueReader.Struct;
                    value.Type = Types.StructPointer;
                    break;

                case 0 when valueReader.IsText:
                    value.ScalarValue = valueReader.Text;
                    value.Type = Types.Text;
                    break;

                case 0 when valueReader.IsUInt16:
                    value.ScalarValue = valueReader.UInt16;
                    value.Type = Types.U16;
                    break;

                case 0 when valueReader.IsUInt32:
                    value.ScalarValue = valueReader.UInt32;
                    value.Type = Types.U32;
                    break;

                case 0 when valueReader.IsUInt64:
                    value.ScalarValue = valueReader.UInt64;
                    value.Type = Types.U64;
                    break;

                case 0 when valueReader.IsUInt8:
                    value.ScalarValue = valueReader.UInt8;
                    value.Type = Types.U8;
                    break;

                case 0 when valueReader.IsVoid:
                    value.Type = Types.Void;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return value;
        }

        void ProcessFields(Schema.Node.Reader reader, TypeDefinition declaringType, List<Field> fields, Pass2State state)
        {
            if (reader.Fields == null)
            {
                return;
            }

            foreach (var fieldReader in reader.Fields)
            {
                var field = new Field()
                {
                    DeclaringType = declaringType,
                    Name = fieldReader.Name,
                    CodeOrder = fieldReader.CodeOrder
                };

                if (fieldReader.DiscriminantValue != Schema.Field.Reader.NoDiscriminant)
                {
                    field.DiscValue = fieldReader.DiscriminantValue;
                }

                switch (0)
                {
                    case 0 when fieldReader.IsGroup:
                        var def = ProcessNode(fieldReader.Group_TypeId, state, TypeTag.Group);
                        field.Type = Types.FromDefinition(def);
                        break;

                    case 0 when fieldReader.IsSlot:
                        field.DefaultValue = ProcessValue(fieldReader.Slot_DefaultValue);
                        field.DefaultValueIsExplicit = fieldReader.Slot_HadExplicitDefault;
                        field.Offset = fieldReader.Slot_Offset;
                        field.Type = ProcessType(fieldReader.Slot_Type, state);
                        field.DefaultValue.Type = field.Type;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                field.InheritFreeGenericParameters();

                fields.Add(field);
            }
        }

        TypeDefinition ProcessInterfaceOrStructTail(TypeDefinition def, Schema.Node.Reader reader, Pass2State state)
        {
            def.IsGeneric = reader.IsGeneric;

            if (def.IsGeneric)
            {
                foreach (var paramReader in reader.Parameters)
                {
                    def.GenericParameters.Add(paramReader.Name);
                }
            }

            ProcessNestedNodes(reader.NestedNodes, state);

            ProcessFields(reader, def, def.Fields, state);

            if (reader.IsInterface)
            {
                foreach (var methodReader in reader.Interface_Methods)
                {
                    var method = new Method()
                    {
                        DeclaringInterface = def,
                        Id = def.Methods.Count,
                        Name = methodReader.Name
                    };
                    foreach (var implicitParameterReader in methodReader.ImplicitParameters)
                    {
                        method.GenericParameters.Add(implicitParameterReader.Name);
                    }
                    state.currentMethod = method;

                    def.Methods.Add(method);

                    var paramNode = IdToNode(methodReader.ParamStructType);
                    var paramType = ProcessParameterList(paramNode, methodReader.ParamBrand, method.Params, state);
                    if (paramType != null)
                    {
                        paramType.SpecialName = SpecialName.MethodParamsStruct;
                        paramType.UsingMethod = method;
                        method.ParamsStruct = Types.FromDefinition(paramType);
                    }
                    else
                    {
                        method.ParamsStruct = method.Params[0].Type;
                    }
                    method.ParamsStruct.InheritFreeParameters(def);
                    method.ParamsStruct.InheritFreeParameters(method);

                    var resultNode = IdToNode(methodReader.ResultStructType);
                    var resultType = ProcessParameterList(resultNode, methodReader.ResultBrand, method.Results, state);
                    if (resultType != null)
                    {
                        resultType.SpecialName = SpecialName.MethodResultStruct;
                        resultType.UsingMethod = method;
                        method.ResultStruct = Types.FromDefinition(resultType);
                    }
                    else
                    {
                        method.ResultStruct = method.Results[0].Type;
                    }
                    method.ResultStruct.InheritFreeParameters(def);
                    method.ResultStruct.InheritFreeParameters(method);
                }

                state.currentMethod = null;
            }
            return def;
        }

        TypeDefinition ProcessStruct(Schema.Node.Reader structReader, TypeDefinition def, Pass2State state)
        {
            def.StructDataWordCount = structReader.Struct_DataWordCount;
            def.StructPointerCount = structReader.Struct_PointerCount;

            if (structReader.Struct_DiscriminantCount > 0)
            {
                def.UnionInfo = new TypeDefinition.DiscriminationInfo(
                    structReader.Struct_DiscriminantCount,
                    16u * structReader.Struct_DiscriminantOffset);
            }

            return ProcessInterfaceOrStructTail(def, structReader, state);
        }

        TypeDefinition ProcessParameterList(Schema.Node.Reader reader, Schema.Brand.Reader brandReader, List<Field> list, Pass2State state)
        {
//# If a named parameter list was specified in the method
//# declaration (rather than a single struct parameter type) then a corresponding struct type is
//# auto-generated. Such an auto-generated type will not be listed in the interface's
//# `nestedNodes` and its `scopeId` will be zero -- it is completely detached from the namespace.
//# (Awkwardly, it does of course inherit generic parameters from the method's scope, which makes
//# this a situation where you can't just climb the scope chain to find where a particular
//# generic parameter was introduced. Making the `scopeId` zero was a mistake.)

            if (!reader.IsStruct)
            {
                throw new InvalidSchemaException("Expected a struct");
            }

            var def = ProcessNode(reader.Id, state, TypeTag.Struct);

            if (reader.ScopeId == 0)
            {
                // Auto-generated => Named parameter list
                foreach (var field in def.Fields) list.Add(field);
                return def;
            }
            else
            {
                // Single, anonymous, struct-typed parameter
                var type = Types.FromDefinition(def);
                ProcessBrand(brandReader, type, state);
                var anon = new Field() { Type = type };
                list.Add(anon);
                return null;
            }
        }

        TypeDefinition ProcessInterface(Schema.Node.Reader ifaceReader, TypeDefinition def, Pass2State state)
        {
            foreach (var superClassReader in ifaceReader.Interface_Superclasses)
            {
                var superClass = ProcessNode(superClassReader.Id, state, TypeTag.Interface);
                def.Superclasses.Add(Types.FromDefinition(superClass));
            }

            return ProcessInterfaceOrStructTail(def, ifaceReader, state);
        }

        TypeDefinition ProcessEnum(Schema.Node.Reader enumReader, TypeDefinition def, Pass2State state)
        {
            foreach (var fieldReader in enumReader.Enumerants)
            {
                var field = new Enumerant()
                {
                    TypeDefinition = def,
                    Literal = fieldReader.Name,
                    CodeOrder = fieldReader.CodeOrder
                };

                if (fieldReader.Ordinal_IsExplicit)
                {
                    field.Ordinal = fieldReader.Ordinal_Explicit;
                }

                def.Enumerants.Add(field);
            }
            return def;
        }

        Value ProcessConst(Schema.Node.Reader constReader, Pass2State state)
        {
            var value = ProcessValue(constReader.Const_Value);
            value.Type = ProcessType(constReader.Const_Type, state);
            return value;
        }

        TypeDefinition ProcessNode(ulong id, Pass2State state, TypeTag tag = default)
        {
            if (!(IdToNode(id, state.isGenerated) is Schema.Node.Reader node)) return null;
            var kind = node.GetKind();
            if (tag == TypeTag.Unknown) tag = kind.GetTypeTag();
            var def = _typeDefMgr.GetExisting(id, tag);
            if (state.processedNodes.Contains(id)) return def;
            state.processedNodes.Add(id);

            switch (kind)
            {
                case NodeKind.Annotation:
                    return def;
                case NodeKind.Const:
                    def.DeclaringElement.Constants.Add(ProcessConst(node, state));
                    return def;
                case NodeKind.Enum:
                    return ProcessEnum(node, def, state);
                case NodeKind.Interface:
                    return ProcessInterface(node, def, state);
                case NodeKind.Struct:
                case NodeKind.Group:
                    return ProcessStruct(node, def, state);
                default:
                    throw new InvalidProgramException($"An unexpected node {node.StrId()} was found during the 2nd schema model building pass.");
            }
        }

        public static SchemaModel Create(Schema.CodeGeneratorRequest.Reader request)
        {
            var model = new SchemaModel(request);
            model.Build();
            return model;
        }
    }

    public enum NodeKind
    {
        Unknown,
        Annotation,
        Const,
        Enum,
        File,
        Interface,
        Struct,
        Group
    }

    public static class SchemaExtensions
    {
        public static string GetName(this Schema.Node.Reader node)
            => node.DisplayName.Substring((int)node.DisplayNamePrefixLength);

        public static string StrId(this Schema.Node.Reader node)
            => $"0x{node.Id.ToString("X")}";

        public static string StrId(this ulong nodeId)
            => $"0x{nodeId.ToString("X")}";

        public static NodeKind GetKind(this Schema.Node.Reader node)
        {
            if (node.IsStruct)
                return node.Struct_IsGroup ? NodeKind.Group : NodeKind.Struct;
            if (node.IsInterface) return NodeKind.Interface;
            if (node.IsEnum) return NodeKind.Enum;
            if (node.IsConst) return NodeKind.Const;
            if (node.IsAnnotation) return NodeKind.Annotation;
            if (node.IsFile) return NodeKind.File;
            return NodeKind.Unknown;
        }

        internal static TypeTag GetTypeTag(this NodeKind kind)
        {
            switch (kind)
            {
                case NodeKind.Enum: return TypeTag.Enum;
                case NodeKind.Interface: return TypeTag.Interface;
                case NodeKind.Struct: return TypeTag.Struct;
                case NodeKind.Group: return TypeTag.Group;
                default: return TypeTag.Unknown;
            }
        }

        internal static TypeTag GetTypeTag(this Schema.Node.Reader node)
            => node.GetKind().GetTypeTag();
    }
}
