using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnomalyDetection.IO
{
    public interface ICsvFileReader
    {
        IEnumerable<dynamic> ReadCsvFileToDynamics(TextReader textReader);
    }

    public class CsvFileReader : ICsvFileReader
    {
        private readonly ILogger log;

        public CsvFileReader(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<dynamic> ReadCsvFileToDynamics(TextReader textReader)
        {
            try
            {
                using (var reader = new CsvHelper.CsvReader(textReader))
                {
                    reader.Configuration.PrepareHeaderForMatch =
                        header => header?.Trim()?.Replace(" ", string.Empty)?.Replace("/", string.Empty);
                    var records = reader.GetRecords<dynamic>().ToArray();

                    log.Debug("Read csv file and parsed {@RowCount} rows", records.Count());

                    return records;
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Failed to read and parse csv file");

                throw e;
            }
        }
    }
}
