using System.Collections.Generic;

namespace CapnpC.CSharp.Generator.Model;

internal interface IHasGenericParameters
{
    List<string> GenericParameters { get; }
}
