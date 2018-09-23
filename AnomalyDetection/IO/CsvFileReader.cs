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
        public IEnumerable<dynamic> ReadCsvFileToDynamics(TextReader textReader)
        {
            using (var reader = new CsvHelper.CsvReader(textReader))
            {
                reader.Configuration.PrepareHeaderForMatch =
                    header => header?.Trim()?.Replace(" ", string.Empty)?.Replace("/", string.Empty);
                return reader.GetRecords<dynamic>().ToArray();
            }
        }
    }
}
