using AnomalyDetection.Models;
using System.Collections.Generic;

namespace AnomalyDetection.IO
{
    public interface IResultsOutputter
    {
        void OutputResults(string sourceFileName, double median, IEnumerable<ParsedInputDataRecord> records);
    }
}