using AnomalyDetection.Algorithm;
using AnomalyDetection.FileParsers;
using AnomalyDetection.IO;
using AnomalyDetection.Settings;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using System;
using System.IO;

namespace AnomalyDetection
{
    public class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigureLogger();

            ConfigureContainer();

            var workingDirectory = GetWorkingDirectory(args);

            try
            {
                RunApplication(workingDirectory);
            } 
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                //.WriteTo.Console()
                .WriteTo.File("log.txt")
                //.WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        private static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterLogger();

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

        private static void RunApplication(DirectoryInfo directory)
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var directoryProcessor = scope.Resolve<IDirectoryProcessor>();
                directoryProcessor.ProcessDirectory(directory);
            }
        }

        private static DirectoryInfo GetWorkingDirectory(string[] args)
        {
            if (args.Length >= 1)
            {
                return new DirectoryInfo(args[0]);
            }
            return new DirectoryInfo(Environment.CurrentDirectory);
        }
    }
}