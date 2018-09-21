using SD.Tools.Algorithmia.PriorityQueues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnomalyDetection {

    public class StreamingMedianCalculator
    {
        private SimplePriorityQueue<double> lowerTerms;
        private SimplePriorityQueue<double> upperTerms;

        public StreamingMedianCalculator() {
            this.lowerTerms = new SimplePriorityQueue<double>((x,y) => x.CompareTo(y));
            this.upperTerms = new SimplePriorityQueue<double>((x,y) => x.CompareTo(y) * -1);
        }

        public void Push(double nextNumber)
        {
            PushToAppropriateSet(nextNumber);
            RebalanceSets();
        }

        public double Median()
        {
            if (IsEvenTermList()) {
                return GetMedianForEvenTermCountList();
            }
            return GetMedianForOddTermCountList();
        }

        private void PushToAppropriateSet(double term) {
            var shouldAddToUpperTerms = term > lowerTerms.Peek();

            if (shouldAddToUpperTerms) {
                upperTerms.Add(term);
            } else {
                lowerTerms.Add(term);
            }
        }

        private void RebalanceSets() {
            RebalanceLowerTerms();
            RebalanceUpperTerms();
        }

        private void RebalanceLowerTerms() {
            var hasTooManyTerms = lowerTerms.Count - upperTerms.Count >= 2;

            if (hasTooManyTerms) {
                upperTerms.Add(lowerTerms.Peek());
                lowerTerms.Remove();
            }
        }

        private void RebalanceUpperTerms() {
            var hasTooManyTerms = upperTerms.Count - lowerTerms.Count >= 1;

            if (hasTooManyTerms) {
                lowerTerms.Add(upperTerms.Peek());
                upperTerms.Remove();
            }
        }

        private bool IsEvenTermList() {
            var totalCount = lowerTerms.Count + upperTerms.Count;
            return totalCount % 2 == 0;
        }

        private double GetMedianForOddTermCountList() {
            return lowerTerms.Peek();
        }

        private double GetMedianForEvenTermCountList() {
            return (lowerTerms.Peek() + upperTerms.Peek()) / 2.0;
        }
    }
}