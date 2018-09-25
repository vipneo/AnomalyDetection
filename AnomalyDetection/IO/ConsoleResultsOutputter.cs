using AnomalyDetection.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnomalyDetection.IO
{
    public class ConsoleResultsOutputter : IResultsOutputter
    {
        private readonly ILogger log;

        public ConsoleResultsOutputter(ILogger log)
        {
            this.log = log;
        }

        public void OutputResults(string sourceFileName, double median, IEnumerable<ParsedInputDataRecord> records)
        {
            foreach (var r in records)
            {
                Console.WriteLine($"{sourceFileName} {r.DateTime} {r.Value} {median}");
            }

            log.Debug("Wrote {@RecordCount} Outlier Records to Console", records.Count());
        }
    }
}