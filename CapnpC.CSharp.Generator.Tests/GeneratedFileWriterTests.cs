using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapnpC.CSharp.Generator.Tests;

[TestClass]
public class GeneratedFileWriterTests
{
    [TestMethod]
    public void WriteCreatesMissingDirectories()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var generatedFile = new FileGenerationResult(
            Path.Combine("go", "style", "nested", "schema.capnp"),
            "// generated content"
        );

        try
        {
            var outputPath = GeneratedFileWriter.Write(generatedFile, tempPath);

            Assert.AreEqual(
                Path.Combine(tempPath, "go", "style", "nested", "schema.capnp.cs"),
                outputPath
            );
            Assert.IsTrue(Directory.Exists(Path.Combine(tempPath, "go", "style", "nested")));
            Assert.IsTrue(File.Exists(outputPath));
            Assert.AreEqual(generatedFile.GeneratedContent, File.ReadAllText(outputPath));
        }
        finally
        {
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
        }
    }
}
