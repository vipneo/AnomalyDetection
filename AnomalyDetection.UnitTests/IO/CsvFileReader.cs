using AnomalyDetection.IO;
using Moq;
using Serilog;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AnomalyDetection.UnitTests.IO
{
    public class CsvFileReaderTests
    {
        [Fact]
        public void GivenCsvData_WhenIReadIt_ThenIGetADynamicWithTheFieldValues()
        {
            var logger = new Mock<ILogger>();
            var sampleData =
@"MeterPoint Code,Serial Number,Plant Code,Date/Time,Data Type,Data Value,Units,Status
210095893,210095893,ED031000001,31/08/2015 00:45:00,Import Wh Total,0.000000,kwh,
210095893,210095893,ED031000001,12/03/2015 00:45:00,Import Wh Total,2.000400,kwh,";
            var sampleDataStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleData)));

            var sut = new CsvFileReader(logger.Object);

            var result = sut.ReadCsvFileToDynamics(sampleDataStream);

            Assert.Equal(2, result.Count());
            Assert.Equal("210095893", result.First().MeterPointCode);
            Assert.Equal("0.000000", result.First().DataValue);
            Assert.Equal("2.000400", result.Last().DataValue);
            Assert.Equal("31/08/2015 00:45:00", result.First().DateTime);
            Assert.Equal("12/03/2015 00:45:00", result.Last().DateTime);
        }
    }
}
