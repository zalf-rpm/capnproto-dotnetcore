﻿using System;
using System.Collections.Generic;
using System.IO;
using CapnpC.CSharp.Generator;

namespace CapnpC.CSharp.MsBuild.Generation;

public class CapnpCodeBehindGenerator : IDisposable
{
    public void Dispose()
    {
    }

    public void InitializeProject(string projectPath)
    {
    }


    public CsFileGeneratorResult GenerateCodeBehindFile(CapnpGenJob job)
    {
        var capnpFile = job.CapnpPath;

        // Works around a weird capnp.exe behavior: When the input file is empty, it will spit out an exception dump
        // instead of a parse error. But the parse error is nice because it contains a generated ID. We want the parse error!
        // Workaround: Generate a temporary file that contains a single line break (such that it is not empty...)
        try
        {
            if (File.Exists(capnpFile) && new FileInfo(capnpFile).Length == 0)
            {
                var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".capnp");

                File.WriteAllText(tempFile, Environment.NewLine);
                try
                {
                    var jobCopy = new CapnpGenJob
                    {
                        CapnpPath = tempFile,
                        WorkingDirectory = job.WorkingDirectory
                    };

                    jobCopy.AdditionalArguments.AddRange(job.AdditionalArguments);

                    return GenerateCodeBehindFile(jobCopy);
                }
                finally
                {
                    File.Delete(tempFile);
                }
            }
        }
        catch
        {
        }

        var args = new List<string>();
        args.AddRange(job.AdditionalArguments);
        args.Add(capnpFile);

        var result = CapnpCompilation.InvokeCapnpAndGenerate(args, job.WorkingDirectory);

        if (result.IsSuccess)
        {
            if (result.GeneratedFiles.Count == 1)
                return new CsFileGeneratorResult(
                    result.GeneratedFiles[0],
                    capnpFile + ".cs",
                    result.Messages);

            return new CsFileGeneratorResult(
                "Code generation produced more than one file. This is not supported.",
                result.Messages);
        }

        switch (result.ErrorCategory)
        {
            case CapnpProcessFailure.NotFound:
                return new CsFileGeneratorResult(
                    "Unable to find capnp.exe - please install capnproto on your system first.");

            case CapnpProcessFailure.BadInput:
                return new CsFileGeneratorResult("Invalid schema", result.Messages);

            case CapnpProcessFailure.BadOutput:
                return new CsFileGeneratorResult(
                    "Internal error: capnp.exe produced a binary code generation request which was not understood by the backend",
                    result.Messages);

            default:
                throw new NotSupportedException("Invalid error category");
        }
    }
}