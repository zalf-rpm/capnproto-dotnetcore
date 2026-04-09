using System;
using System.IO;

namespace CapnpC.CSharp.Generator;

internal static class GeneratedFileWriter
{
    internal static string GetOutputPath(
        FileGenerationResult generatedFile,
        string baseDirectory = null
    )
    {
        generatedFile = generatedFile ?? throw new ArgumentNullException(nameof(generatedFile));

        var outputPath = generatedFile.CapnpFilePath + ".cs";

        if (!string.IsNullOrWhiteSpace(baseDirectory) && !Path.IsPathRooted(outputPath))
            outputPath = Path.Combine(baseDirectory, outputPath);

        return outputPath;
    }

    internal static string Write(FileGenerationResult generatedFile, string baseDirectory = null)
    {
        generatedFile = generatedFile ?? throw new ArgumentNullException(nameof(generatedFile));

        if (!generatedFile.IsSuccess)
            throw new InvalidOperationException("Cannot write an unsuccessful generation result");

        var outputPath = GetOutputPath(generatedFile, baseDirectory);
        var directoryPath = Path.GetDirectoryName(outputPath);

        if (!string.IsNullOrEmpty(directoryPath))
            Directory.CreateDirectory(directoryPath);

        File.WriteAllText(outputPath, generatedFile.GeneratedContent);

        return outputPath;
    }
}
