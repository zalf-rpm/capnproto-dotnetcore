@0x9273388a9624d431;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("MsBuildGenerationTest.IssueGenGeneric");

interface GenericInterface(T) {
  interface NestedInterface {
    method @0 ();

    # Capability returned from an interface nested in a generic one: the pipelining
    # extension method has to declare the enclosing type's parameter T.
    child @1 () -> (msg :Msg, child :NestedInterface);
  }

  getNested @0 () -> (nested :NestedInterface);
  
  struct Msg {
     val @0 :T;
  }
  
  read @1 () -> (msg :Msg);
  
  endpoints @2 () -> (nested :NestedInterface, self :GenericInterface(T));
}
