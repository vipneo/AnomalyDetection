using AnomalyDetection.Algorithm;
using Moq;
using Serilog;
using Xunit;

namespace AnomalyDetection.UnitTests.Algorithm
{
    public class OutlierRecordFilterTests
    {
        [Theory]
        [InlineData(10.0, 0.2, 8.0)]
        [InlineData(22.3, 0.3, 15.61)]
        public void GivenAMedianAndAnOutlierPercentage_WhenIGetTheLowerBound_ThenIGetTheCorrectValue(double median, double outlierPercentage, double expectedLowerBound)
        {
            var logger = new Mock<ILogger>();
            var sut = new OutlierRecordFilter(logger.Object);

            Assert.Equal(expectedLowerBound, sut.GetLowerBound(median, outlierPercentage));
        }

        [Theory]
        [InlineData(10.0, 0.2, 12.0)]
        [InlineData(22.5, 0.5, 33.75)]
        public void GivenAMedianAndAnOutlierPercentage_WhenIGetTheUpperBound_ThenIGetTheCorrectValue(double median, double outlierPercentage, double expectedUpperBound)
        {
            var logger = new Mock<ILogger>();
            var sut = new OutlierRecordFilter(logger.Object);

            Assert.Equal(expectedUpperBound, sut.GetUpperBound(median, outlierPercentage));
        }

        [Theory]
        [InlineData(454.22, 500, 700, true)]
        [InlineData(454.22, 400, 700, false)]
        [InlineData(454.22, 400, 420, true)]
        [InlineData(454.22, 454.22, 420, true)]
        [InlineData(454.22, 300, 454.22, true)]
        public void GivenAValueAndLowerAndUpperBounds_WhenICheckIfTheValueIsAnOutlier_ThenIGetTheCorrectResult(double value, double lowerBound, double upperBound, bool expected)
        {
            var logger = new Mock<ILogger>();
            var sut = new OutlierRecordFilter(logger.Object);

            Assert.Equal(expected, sut.IsOutlierValue(value, lowerBound, upperBound));
        }

        [Theory]
        [InlineData(0, 0, 0, false)]
        [InlineData(2, 0, 0, true)]
        [InlineData(-2, 0, 0, true)]
        public void GivenAValueAndBothBoundsAreZero_WhenICheckIfTheValueIsAnOutlier_ThenIGetTheCorrectResult(double value, double lowerBound, double upperBound, bool expected)
        {
            var logger = new Mock<ILogger>();
            var sut = new OutlierRecordFilter(logger.Object);

            Assert.Equal(expected, sut.IsOutlierValue(value, lowerBound, upperBound));
        }
    }
}
