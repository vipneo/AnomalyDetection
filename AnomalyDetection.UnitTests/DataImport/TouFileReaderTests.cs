using AnomalyDetection.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace AnomalyDetection.UnitTests.DataImport
{
    public class TouFileReaderTests
    {
        [Fact]
        public void GivenACsvTextReader_WhenWeParseAnTouFile_ThenWeGetImportRecordsWithTheCorrectValues()
        {
            var sampleData =
@"MeterPoint Code,Serial Number, Plant Code,Date / Time,Data Type, Energy, Maximum Demand,Time of Max Demand, Units, Status, Period, DLS Active,Billing Reset Count,Billing Reset Date / Time,Rate
212621147,212621147,ED011300247,11 / 09 / 2015 00:41:07,Export Wh Total,378331.600000,1118.448000,30 / 12 / 1899 00:00:00,kwh,.....R....,Total,False,26,01 / 09 / 2015 00:00:00,Unified
212621147,212621147,ED011300247,12 / 09 / 2015 00:41:07,Export Wh Total,2.600300,1118.448000,30 / 12 / 1899 00:00:00,kwh,.....R....,Total,False,26,01 / 09 / 2015 00:00:00,Unified";
            var sampleDataStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleData)));

            var sut = new TouFileReader();

            var expected = new List<ImportRecord>
            {
                new ImportRecord(new DateTime(2015, 9, 11, 0, 41, 7), 378331.6),
                new ImportRecord(new DateTime(2015, 9, 12, 0, 41, 7), 2.6003)
            };

            Assert.Equal(expected, sut.Parse(sampleDataStream));
        }
    }
}
