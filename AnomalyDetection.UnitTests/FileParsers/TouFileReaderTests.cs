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
    public class TouFileReaderTests
    {
        [Fact]
        public void GivenACsvTextReader_WhenWeParseAnTouFile_ThenWeGetImportRecordsWithTheCorrectValues()
        {        
            var logger = new Mock<ILogger>();
            var mockDataStream = new Mock<TextReader>();
            var mockCsvFileReader = new Mock<ICsvFileReader>();

            var sampleDataItem1 = new ExpandoObject();
            sampleDataItem1.TryAdd("DateTime", "11/ 09/2015 00:41:07");
            sampleDataItem1.TryAdd("Energy", "378331.600000");

            var sampleDataItem2 = new ExpandoObject();
            sampleDataItem2.TryAdd("DateTime", "12/09/2015 00:41:07");
            sampleDataItem2.TryAdd("Energy", "2.600300");

            mockCsvFileReader
                .Setup(o => o.ReadCsvFileToDynamics(mockDataStream.Object))
                .Returns(new List<dynamic>
                {
                    sampleDataItem1,
                    sampleDataItem2
                });

            var sut = new TouFileParser(mockCsvFileReader.Object, logger.Object);

            var expected = new List<ParsedInputDataRecord>
                {
                    new ParsedInputDataRecord(new DateTime(2015, 9, 11, 0, 41, 7), 378331.6),
                    new ParsedInputDataRecord(new DateTime(2015, 9, 12, 0, 41, 7), 2.6003)
                };

            Assert.Equal(expected, sut.ReadAndParse(mockDataStream.Object));
        }
    }
}
