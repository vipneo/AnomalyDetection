using AnomalyDetection.Algorithm;
using Moq;
using Serilog;
using Xunit;

namespace AnomalyDetection.UnitTests.Algorithm
{
    public class StreamingMedianCalculatorTests
    {
        [Fact]
        public void GivenAnEmptyState_WhenASingleNumberIsPushed_ThenTheMedianIsThatNumber()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var onlyNumber = 5.0;

            sut.Push(onlyNumber);

            Assert.Equal(onlyNumber, sut.Median());
        }

        [Fact]
        public void GivenAnEmptyState_WhenTwoNumbersArePushed_ThenTheMedianIsAverageOfThoseNumbers()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var firstNumber = 5.0;
            var secondNumber = 15.0;

            sut.Push(firstNumber);
            sut.Push(secondNumber);

            var expected = (firstNumber + secondNumber) / 2.0;

            Assert.Equal(expected, sut.Median());
        }

        [Fact]
        public void GivenAnEmptyState_WhenThreeNumbersArePushed_AndTheyAreInOrder_ThenTheMedianIsTheSecondNumber()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var firstNumber = 5.0;
            var secondNumber = 15.0;
            var thirdNumber = 60.0;

            sut.Push(firstNumber);
            sut.Push(secondNumber);
            sut.Push(thirdNumber);

            Assert.Equal(secondNumber, sut.Median());
        }
        
        [Fact]
        public void GivenAnEmptyState_WhenFourNumbersArePushed_AndTheyAreInOrder_ThenTheMedianIsTheAverageOfTheSecondAndThirdNumbers()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var firstNumber = 5.0;
            var secondNumber = 15.0;
            var thirdNumber = 60.0;
            var forthNumber = 74.0;

            sut.Push(firstNumber);
            sut.Push(secondNumber);
            sut.Push(thirdNumber);
            sut.Push(forthNumber);

            var expected = (secondNumber + thirdNumber) / 2.0;

            Assert.Equal(expected, sut.Median());
        }
        
        [Fact]
        public void GivenAnEmptyState_WhenThreeNumbersArePushed_AndTheyAreNotInOrder_ThenTheMedianIsTheSecondNumber()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var lowestNumber = 5.0;
            var middleNumber = 15.0;
            var highestNumber = 60.0;

            sut.Push(middleNumber);
            sut.Push(highestNumber);
            sut.Push(lowestNumber);

            Assert.Equal(middleNumber, sut.Median());
        }
        
        [Fact]
        public void GivenAnEmptyState_WhenFourNumbersArePushed_AndTheyAreNotInOrder_ThenTheMedianIsTheAverageOfTheSortedSecondAndThirdNumbers()
        {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var lowestNumber = 5.0;
            var secondLowestNumber = 15.0;
            var secondHigestNumber = 60.0;
            var highestNumber = 74.0;

            sut.Push(secondLowestNumber);
            sut.Push(lowestNumber);
            sut.Push(secondHigestNumber);
            sut.Push(highestNumber);

            var expected = (secondLowestNumber + secondHigestNumber) / 2.0;

            Assert.Equal(expected, sut.Median());
        }

        [Fact]
        public void GivenAnEmptyState_WhenFourNumbersArePushed_AndTheFirstTwoNumbersAreEqual_ThenTheMedianIsTheAverageOfTheSortedSecondAndThirdNumbers() {
            var logger = new Mock<ILogger>();
            var sut = new StreamingMedianCalculator(logger.Object);
            var lowestNumber = 5.0;
            var secondLowestNumber = 5.0;
            var secondHigestNumber = 60.0;
            var highestNumber = 74.0;

            sut.Push(secondLowestNumber);
            sut.Push(lowestNumber);
            sut.Push(secondHigestNumber);
            sut.Push(highestNumber);

            var expected = (secondLowestNumber + secondHigestNumber) / 2.0;

            Assert.Equal(expected, sut.Median());
        }
    }
}
