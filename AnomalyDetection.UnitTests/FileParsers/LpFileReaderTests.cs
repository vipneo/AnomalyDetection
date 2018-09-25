using AnomalyDetection.FileParsers;
using AnomalyDetection.IO;
using AnomalyDetection.Models;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Xunit;

namespace AnomalyDetection.UnitTests.FileParsers
{
    public class LpFileReaderTests
    {
        [Fact]
        public void GivenACsvTextReader_WhenWeParseAnLpFile_ThenWeGetImportRecordsWithTheCorrectValues()
        {
            var logger = new Mock<ILogger>();
            var mockDataStream = new Mock<TextReader>();
            var mockCsvFileReader = new Mock<ICsvFileReader>();

            var sampleDataItem1 = new ExpandoObject();
            sampleDataItem1.TryAdd("DateTime", "31/08/2015 00:45:00");
            sampleDataItem1.TryAdd("DataValue", "0.000000");

            var sampleDataItem2 = new ExpandoObject();
            sampleDataItem2.TryAdd("DateTime", "12/03/2015 00:45:00");
            sampleDataItem2.TryAdd("DataValue", "2.000400");

            mockCsvFileReader
                .Setup(o => o.ReadCsvFileToDynamics(mockDataStream.Object))
                .Returns(new List<dynamic>
                {
                    sampleDataItem1,
                    sampleDataItem2
                });

            var sut = new LpFileParser(mockCsvFileReader.Object, logger.Object);

            var expected = new List<ParsedInputDataRecord>
            {
                new ParsedInputDataRecord(new DateTime(2015, 8, 31, 0, 45, 0), 0),
                new ParsedInputDataRecord(new DateTime(2015, 3, 12, 0, 45, 0), 2.0004)
            };

            Assert.Equal(expected, sut.ReadAndParse(mockDataStream.Object));
        }
    }
}
