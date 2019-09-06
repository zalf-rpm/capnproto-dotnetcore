using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Capnpc.Csharp.MsBuild.Generation
{
    public class GenerateCapnpFileCodeBehindTask : Task
    {
        public GenerateCapnpFileCodeBehindTask()
        {
            CodeBehindGenerator = new CapnpFileCodeBehindGenerator(Log);
        }

        public ICapnpcCsharpGenerator CodeBehindGenerator { get; set; }

        [Required]
        public string ProjectPath { get; set; }

        public string ProjectFolder => Path.GetDirectoryName(ProjectPath);

        public ITaskItem[] CapnpFiles { get; set; }

        public string WorkingDirectory { get; set; }

        public string AdditionalOptions { get; set; }

        [Output]
        public ITaskItem[] GeneratedFiles { get; private set; }

        public override bool Execute()
        {
            try
            {
                try
                {
                    var currentProcess = Process.GetCurrentProcess();

                    Log.LogWithNameTag(Log.LogMessage, $"process: {currentProcess.ProcessName}, pid: {currentProcess.Id}, CD: {Environment.CurrentDirectory}");

                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Log.LogWithNameTag(Log.LogMessage, "  " + assembly.FullName);
                    }
                }
                catch (Exception e)
                {
                    Log.LogWithNameTag(Log.LogMessage, $"Error when dumping process info: {e}");
                }

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                var generator = CodeBehindGenerator ?? new CapnpFileCodeBehindGenerator(Log);

                Log.LogWithNameTag(Log.LogMessage, "Starting GenerateFeatureFileCodeBehind");

                var capnpFiles = CapnpFiles?.Select(i => i.ItemSpec).ToList() ?? new List<string>();

                var generatedFiles = generator.GenerateFilesForProject(
                    ProjectPath,
                    capnpFiles,
                    ProjectFolder,
                    WorkingDirectory,
                    AdditionalOptions);

                GeneratedFiles = generatedFiles.Select(file => new TaskItem { ItemSpec = file }).ToArray();

                return !Log.HasLoggedErrors;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException is FileLoadException fle)
                    {
                        Log?.LogWithNameTag(Log.LogError, $"FileLoadException Filename: {fle.FileName}");
                        Log?.LogWithNameTag(Log.LogError, $"FileLoadException FusionLog: {fle.FusionLog}");
                        Log?.LogWithNameTag(Log.LogError, $"FileLoadException Message: {fle.Message}");
                    }

                    Log?.LogWithNameTag(Log.LogError, e.InnerException.ToString());
                }

                Log?.LogWithNameTag(Log.LogError, e.ToString());
                return false;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Log.LogWithNameTag(Log.LogMessage, args.Name);
            

            return null;
        }

    }


}