using AnomalyDetection.IO;
using AnomalyDetection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnomalyDetection.FileParsers
{
    public class LpFileParser : IFileParser
    {
        private readonly ICsvFileReader csvFileReader;

        public LpFileParser(ICsvFileReader csvFileReader)
        {
            this.csvFileReader = csvFileReader;
        }

        public bool CanReadFile(FileInfo fileInfo)
        {
            return fileInfo.Name.StartsWith("LP") && fileInfo.Extension.Equals(".csv");
    }

    public IEnumerable<ParsedInputDataRecord> ReadAndParse(TextReader textReader)
        {
            var raw = csvFileReader.ReadCsvFileToDynamics(textReader);
            return raw.Select(ParseRecord).ToList();
        }

        public ParsedInputDataRecord ParseRecord(dynamic raw)
        {
            DateTime date = DateTime.Parse(raw.DateTime);
            double value = Convert.ToDouble(raw.DataValue);
            return new ParsedInputDataRecord(date, value);
        }
    }
}
