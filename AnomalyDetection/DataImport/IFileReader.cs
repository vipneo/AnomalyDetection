using System.Collections.Generic;
using System.IO;

namespace AnomalyDetection.DataImport
{
    public interface IFileReader
    {
        IEnumerable<ImportRecord> Parse(TextReader textReader);
    }
}
