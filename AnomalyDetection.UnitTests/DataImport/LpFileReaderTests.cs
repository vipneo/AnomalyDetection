using AnomalyDetection.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace AnomalyDetection.UnitTests.DataImport
{
    public class LpFileReaderTests
    {
        [Fact]
        public void GivenACsvTextReader_WhenWeParseAnLpFile_ThenWeGetImportRecordsWithTheCorrectValues()
        {
            var sampleData =
@"MeterPoint Code,Serial Number,Plant Code,Date/Time,Data Type,Data Value,Units,Status
210095893,210095893,ED031000001,31/08/2015 00:45:00,Import Wh Total,0.000000,kwh,
210095893,210095893,ED031000001,12/03/2015 00:45:00,Import Wh Total,2.000400,kwh,";
            var sampleDataStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleData)));

            var sut = new LpFileReader();

            var expected = new List<ImportRecord>
            {
                new ImportRecord(new DateTime(2015, 8, 31, 0, 45, 0), 0),
                new ImportRecord(new DateTime(2015, 3, 12, 0, 45, 0), 2.0004)
            };

            Assert.Equal(expected, sut.Parse(sampleDataStream));
        }
    }
}
