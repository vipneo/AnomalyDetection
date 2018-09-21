using System;
using System.Collections.Generic;
using System.Linq;

namespace AnomalyDetection {

    public class StreamingMedianCalculator
    {
        private List<double> numbers;

        public StreamingMedianCalculator() {
            this.numbers = new List<double>();
        }

        public void Push(double nextNumber)
        {
            numbers.Add(nextNumber);
        }

        public double Median()
        {
            return numbers.Average();
        }
    }
}