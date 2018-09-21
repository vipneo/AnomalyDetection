using System;
using Xunit;

namespace AnomalyDetection.UnitTests
{
    public class StreamingMedianCalculatorTests
    {
        [Fact]
        public void GivenAnEmptyStateAddANewNumberAndMedianIsThatNumber()
        {
            var sut = new StreamingMedianCalculator();
            var onlyNumber = 5;

            sut.Push(onlyNumber);

            Assert.Equal(onlyNumber, sut.Median());
        }

        [Fact]
        public void GivenAnEmptyStateAddTwoNewNumbersAndMedianIsAverageOfThoseNumbers()
        {
            var sut = new StreamingMedianCalculator();
            var firstNumber = 5;
            var secondNumber = 15;

            sut.Push(firstNumber);
            sut.Push(secondNumber);

            var expected = (firstNumber + secondNumber) / 2;

            Assert.Equal(expected, sut.Median());
        }
    }
}
