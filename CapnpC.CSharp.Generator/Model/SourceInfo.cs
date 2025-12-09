using System.Collections.Generic;

namespace CapnpC.CSharp.Generator.Model;

internal class SourceInfo
{
    public string DocComment { get; set; }
    public IReadOnlyList<string> MemberDocComments { get; set; }
}
