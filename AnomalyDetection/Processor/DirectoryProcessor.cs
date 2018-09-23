using System.IO;

namespace AnomalyDetection.IO
{
    public interface IDirectoryProcessor
    {
        void ProcessDirectory(DirectoryInfo directory);
    }

    public class DirectoryProcessor: IDirectoryProcessor
    {
        private readonly IFileProcessor fileProcessor;

        public DirectoryProcessor(IFileProcessor fileProcessor)
        {
            this.fileProcessor = fileProcessor;
        }

        public void ProcessDirectory(DirectoryInfo directory)
        {
            var files = directory.GetFiles();

            foreach (var file in files)
            {
                fileProcessor.ProcessFile(file);
            }
        }
    }
}