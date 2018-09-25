using AnomalyDetection.IO;
using AnomalyDetection.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnomalyDetection.FileParsers
{
    public class TouFileParser : IFileParser
    {
        private readonly ICsvFileReader csvFileReader;
        private readonly ILogger log;

        public TouFileParser(ICsvFileReader csvFileReader, ILogger log)
        {
            this.csvFileReader = csvFileReader;
            this.log = log;
        }

        public bool CanReadFile(FileInfo fileInfo)
        {
            var startsWithTOU = fileInfo.Name.StartsWith("TOU");
            var hasCsvExtension = fileInfo.Extension.Equals(".csv");
            var canReadFile = startsWithTOU && hasCsvExtension;

            if (canReadFile)
                log.Debug("TouFileParser can read file {@Filename}", fileInfo.Name);
            else
                log.Verbose("TouFileParser cannot read file {@Filename} - Starts with 'TOU': {@StartsWithTOU}, Has csv extension: {@HasCsvExtension}", fileInfo.Name, startsWithTOU, hasCsvExtension);

            return canReadFile;
        }

        public IEnumerable<ParsedInputDataRecord> ReadAndParse(TextReader textReader)
        {
            var raw = csvFileReader.ReadCsvFileToDynamics(textReader);
            return raw.Select(ParseRecord).ToList();
        }

        public ParsedInputDataRecord ParseRecord(dynamic raw)
        {
            try
            {
                DateTime date = DateTime.Parse(raw.DateTime);
                double value = Convert.ToDouble(raw.Energy);
                var record = new ParsedInputDataRecord(date, value);

                log.Verbose("Parsed record {@Record} from {@RawData}", record, raw);

                return record;
            }
            catch (Exception e)
            {
                log.Error(e, "Failed to parse {@RawData}", raw);
                throw e;
            }
        }
    }
}
