# AnomalyDetection

Author: Adam Minchenton

## Purpose

The purpose of this application is to process a directory containing files with numeric data values, and then for each file calculate the median value of those values and output a list of all the values that fall 20% or more away from the median of that file to the Console. Currently LP and TOU csv files are supported.

## Task Notes For Reviewer

1. Currently the application processes an entire directory and all the files it contains. As part of a deployed Microservices approach, the directory processor would be broken out to just add file processing events to a queue, and then multiple file processors could action each file individually, allowing the process to be scaled out horizontally for file processing.

2. If another file type is to be supported, then the IFileParser Interface needs to be implemented. Then add a registration for this new type, as an IFileParser in the Autofac container. Then it will automatically be injected, along with the other existing IFileParsers implementations for LP and TOU files.

3. This Console Application does not have command line help, this should be added so that the application can instruct the user how to use it.

4. Logging has not been implemented. Using a logging library such as Serilog is recommended, as it would allow the application to be configured to log to various sources, depending on how it is deployed. Eg. Adhoc running by user vs automated process.

5. Currently the 20% value is hardcoded, abit in an injected setting type. This could be exposed as a command line and\or settings file value to allow the user to override the value if required.

6. Performance is fast enough using a console window as output, with files of the size of the samples given. If this process is automated, or much larger input files are expected, specific performance testing (both manual and automated) would be recommened.

7. High value unit test cases have been implemented for the calculation of the Median, Outliers and Parsing CSV files. Additional unit test cases can be added to ensure the remaining classes work as expected, however are of lower value than those implemented.

## How to Build & Run Application

The application is a .Net Core 2.1 Console Application. You will need to have the .Net Core 2.1 SDK installed to build the application. You can either use Visual Studio 2017, Visual Studio Code, or the dotnet cli tool to build this application.

### Using dotnet cli

1. Open a console window and navigate to the root directory of this repository
2. Run ```dotnet run -p AnomalyDetection "c:\dev\ermpower\sample files"```, where ```"c:\dev\ermpower\sample files"``` is the location of the files you want to process.

## How to Run Tests

The tests for this application can be run with either Visual Studio 2017, Visual Studio Code or the dotnet cli tool.

### Using dotnet cli

1. Open a console window and navigate to the root directory of this repository
2. Run ```dotnet test AnomalyDetection.UnitTests```
3. Test Results will display in console
