using System.IO;
using CapnpC.CSharp.Generator.Tests;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapnpC.CSharp.MsBuild.Generation.Tests;

[TestClass]
public class GenerateCapnpFileCodeBehindTaskTest
{
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
        Assert.AreEqual(1, task.GeneratedFiles.Length);
        var csPath = Path.Combine(tmpPath, task.GeneratedFiles[0].ItemSpec);
        Assert.AreEqual(capnpPath + ".cs", csPath);
        Assert.IsTrue(File.Exists(csPath));
    }
}