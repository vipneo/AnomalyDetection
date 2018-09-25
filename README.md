# AnomalyDetection

Author: Adam Minchenton

## Purpose

This application checks a directory of files, processing those that match the expected types. It then processes each file with the appropriate handler for its format, extracting a data value. This data value is then used to calculate a median for the entire file. After calculating the file's median, all the records in the file that are 20% away from the median are output to the console. 

## Technologies Used

**Application:** .Net Core 2.1, C#, Autofac, Serilog, CsvHelper, SD.Tools.Algorithmia

**Testing:** xUnit, Moq 

## Task Notes For Reviewer

1. Currently the application processes an entire directory and all the files it contains. As part of a deployed Microservices approach, the directory processor would likely be broken out into a process that just adds files to be processed events to a queue. Then multiple file processors could action events from the queue, allowing the process to be scaled out horizontally. This was not required yet, as all output is currently piped to a console window.

2. Currently 'LP' and 'TOU' files are supported, and the application can be extended to support additional file types by implementing a new IFileParser. This new IFileParser just needs to be added as an additional registration in the Autofac container. After adding the registration it will automatically be injected into the FileProcessor.

3. This Console Application does not have command line help, this should be added so that the application can instruct the user how to use it.

4. Currently the 20% value is hardcoded, abit in an injected setting type. This could be exposed as a command line and\or configuration file setting to allow the user to override the value if required.

5. Performance is fast enough using a console window as output, with files of the size of the samples given. If this process is automated, or much larger input files are expected, specific performance testing (both manual and automated) would be recommened.

## How to Build & Run Application

The application is a .Net Core 2.1 Console Application. You will need to have the .Net Core 2.1 SDK installed to build the application. You can either use Visual Studio 2017, Visual Studio Code, or the dotnet cli tool to build this application.

### Using dotnet cli to build and run

1. Open a console window and navigate to the root directory of this repository
2. Run ```dotnet run -p AnomalyDetection ".\sampledata"```, where ```".\sampledata"``` is the location of the files you want to process.

### Using Visual Studio to build and run

1. Open the ```AnomalyDetection.sln``` solution
2. Open the properties for the AnomalyDetection Project
3. Open the Debug Properties tab
4. Update the Application Arguments with the path to the files you want process. By default it uses the included sample files.
5. Build and Run the AnomalyDetection Project

## How to Run Tests

The tests for this application can be run with either Visual Studio 2017, Visual Studio Code or the dotnet cli tool.

### Using dotnet cli to run tests

1. Open a console window and navigate to the root directory of this repository
2. Run ```dotnet test AnomalyDetection.UnitTests```
3. Test Results will display in console

