using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnomalyDetection.DataImport
{
    public class TouFileReader : IFileReader
    {
        public IEnumerable<ImportRecord> Parse(TextReader textReader)
        {
            using (var reader = new CsvHelper.CsvReader(textReader))
            {
                reader.Configuration.PrepareHeaderForMatch =
                    header => header?.Trim()?.Replace(" ", string.Empty)?.Replace("/", string.Empty);
                var raw = reader.GetRecords<dynamic>();
                var records = raw.Select(r => {
                    DateTime date = DateTime.Parse(r.DateTime);
                    double value = Convert.ToDouble(r.Energy);
                    return new ImportRecord(date, value);
                }).ToList();
                return records;
            }
        }
    }
}
