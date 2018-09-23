using AnomalyDetection.Algorithm;
using AnomalyDetection.FileParsers;
using AnomalyDetection.IO;
using AnomalyDetection.Settings;
using Autofac;
using System;
using System.IO;

namespace AnomalyDetection
{
    public class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigureContainer();
            RunApplication();
        }

        private static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DirectoryProcessor>().As<IDirectoryProcessor>();
            builder.RegisterType<FileProcessor>().As<IFileProcessor>();
            builder.RegisterType<OutlierRecordFilter>().As<IOutlierRecordFilter>();
            builder.RegisterType<StreamingMedianCalculator>().As<IStreamingMedianCalculator>();
            builder.RegisterType<TouFileParser>().As<IFileParser>();
            builder.RegisterType<LpFileParser>().As<IFileParser>();
            builder.RegisterType<CsvFileReader>().As<ICsvFileReader>();
            builder.RegisterType<ConsoleResultsOutputter>().As<IResultsOutputter>();
            builder.RegisterType<OutlierPercentageSetting>().As<IOutlierPercentageSetting>();

            Container = builder.Build();
        }

        private static void RunApplication()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var directory = AppContext.BaseDirectory;
                var directoryInfo = new DirectoryInfo(directory);

                var directoryProcessor = scope.Resolve<IDirectoryProcessor>();
                directoryProcessor.ProcessDirectory(directoryInfo);
            }
        }
    }
}