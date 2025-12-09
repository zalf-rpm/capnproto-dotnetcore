using System.Collections.Generic;
using System.IO;
using System.Linq;
using CapnpC.CSharp.Generator;
using CapnpC.CSharp.Generator.Tests;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapnpC.CSharp.MsBuild.Generation.Tests;

[TestClass]
public class GenerateCapnpFileCodeBehindTaskTest
{
    [TestInitialize]
    public void Setup()
    {
        CapnpCompilation.ExternalCapnpInvoker = MockInvoker;
    }

    [TestCleanup]
    public void Cleanup()
    {
        CapnpCompilation.ExternalCapnpInvoker = null;
    }

    private GenerationResult MockInvoker(IEnumerable<string> args, string workingDir)
    {
        var generatedFiles = new List<FileGenerationResult>();
        var fileArg = args.FirstOrDefault(a => a.EndsWith(".capnp"));
        if (fileArg != null)
        {
            var genFile = new FileGenerationResult(fileArg, "// Mock generated content");
            generatedFiles.Add(genFile);
        }
        return new GenerationResult(generatedFiles);
    }

    private string LoadResourceContent(string name)
    {
        using (var stream = CodeGeneratorSteps.LoadResource("UnitTest1.capnp"))
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    [TestMethod]
    public void ExecutionWithoutParameters()
    {
        var task = new GenerateCapnpFileCodeBehindTask();
        task.BuildEngine = new BuildEngineMock();
        task.Execute();
        // Should not crash. Should Execute() return true or false if there is no input?
    }

    [TestMethod]
    public void SimpleGeneration()
    {
        var capnpFile = "UnitTask1.capnp";
        var content = LoadResourceContent(capnpFile);
        var tmpPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tmpPath);
        var capnpPath = Path.Combine(tmpPath, capnpFile);
        File.WriteAllText(capnpPath, content);

        var task = new GenerateCapnpFileCodeBehindTask();
        task.BuildEngine = new BuildEngineMock();
        task.ProjectPath = Path.Combine(tmpPath, "doesnotneedtoexist.csproj");
        task.CapnpFiles = new ITaskItem[1] { new TaskItemMock { ItemSpec = capnpPath } };
        Assert.IsTrue(task.Execute());
        Assert.IsNotNull(task.GeneratedFiles);
        Assert.HasCount(1, task.GeneratedFiles);
        var csPath = Path.Combine(tmpPath, task.GeneratedFiles[0].ItemSpec);
        Assert.AreEqual(capnpPath + ".cs", csPath);
        Assert.IsTrue(File.Exists(csPath));
    }
}
