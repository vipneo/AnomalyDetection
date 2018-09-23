using AnomalyDetection.Models;
using System.Collections.Generic;
using System.IO;

namespace AnomalyDetection.FileParsers
{
    public interface IFileParser
    {
        bool CanReadFile(FileInfo fileInfo);

        IEnumerable<ParsedInputDataRecord> ReadAndParse(TextReader textReader);
    }
}
