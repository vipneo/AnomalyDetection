using AnomalyDetection.Models;
using System;
using System.Collections.Generic;

namespace AnomalyDetection.IO
{
    public class ConsoleResultsOutputter : IResultsOutputter
    {
        public void OutputResults(string sourceFileName, double median, IEnumerable<ParsedInputDataRecord> records)
        {
            foreach (var r in records)
            {
                Console.WriteLine($"{sourceFileName} {r.DateTime} {r.Value} {median}");
            }
        }
    }
}