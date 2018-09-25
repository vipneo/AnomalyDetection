using Serilog;
using System;
using System.IO;
using System.Linq;

namespace AnomalyDetection.IO
{
    public interface IDirectoryProcessor
    {
        void ProcessDirectory(DirectoryInfo directory);
    }

    public class DirectoryProcessor: IDirectoryProcessor
    {
        private readonly IFileProcessor fileProcessor;
        private readonly ILogger log;

        public DirectoryProcessor(IFileProcessor fileProcessor, ILogger log)
        {
            this.fileProcessor = fileProcessor;
            this.log = log;
        }

        public void ProcessDirectory(DirectoryInfo directory)
        {
            try
            {
                log.Information("Processing Directory {@Directory}", directory.FullName);
                var files = directory.GetFiles();
                log.Information("Found {@FileCount} in directory", files.Length);
                log.Verbose("Files found are: {@Filenames}", files.Select(f => f.Name));

                foreach (var file in files)
                {
                    try
                    {
                        fileProcessor.ProcessFile(file);
                    }
                    catch (Exception e)
                    {
                        log.Error(e, "Failure processing file {@Filename}", file.Name);
                        throw e;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error(e, "Directory {@Directory} could not be found", directory.Name);
                throw e;
            }
        }
    }
}