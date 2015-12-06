using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConvertToMp3
{
    public class Converter
    {
        public int ProcessedSourceFiles { get; private set; }

        public int TotalSourceFiles { get; private set; }

        public string State { get; private set; }

        public void ConvertFiles(List<string> sourceFilePaths, bool deleteSourceFile)
        {
            TotalSourceFiles = sourceFilePaths.Count;
            foreach (var sourceFilePath in sourceFilePaths)
            {
                ConvertFile(sourceFilePath, deleteSourceFile);
                ProcessedSourceFiles++;
            }
        }

        private void ConvertFile(string sourceFilePath, bool deleteSourceFile)
        {
            var sourceFileInfo = new FileInfo(sourceFilePath);
            var destFilePath = sourceFilePath.Replace(sourceFileInfo.Extension, ".mp3");

            if (File.Exists(destFilePath))
            {
                File.Delete(destFilePath);
            }

            var processStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = Properties.Settings.Default.FileName,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = $"-i \"{sourceFilePath}\" -ab 320k \"{destFilePath}\""
            };

            State = $"Converting {sourceFileInfo.Name} ...";

            using (var process = Process.Start(processStartInfo))
            {
                process?.WaitForExit();
            }

            if (deleteSourceFile && File.Exists(destFilePath))
            {
                File.Delete(sourceFilePath);
            }
        }
    }
}