using SD.Tools.Algorithmia.PriorityQueues;
using Serilog;

namespace AnomalyDetection.Algorithm
{
    public interface IStreamingMedianCalculator
    {
        void Push(double nextNumber);
        double Median();
    }

    public class StreamingMedianCalculator : IStreamingMedianCalculator
    {
        private SimplePriorityQueue<double> lowerTerms;
        private SimplePriorityQueue<double> upperTerms;
        private readonly ILogger log;

        public StreamingMedianCalculator(ILogger log) {
            lowerTerms = new SimplePriorityQueue<double>((x,y) => x.CompareTo(y));
            upperTerms = new SimplePriorityQueue<double>((x,y) => x.CompareTo(y) * -1);
            this.log = log;
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
            return GetMedianForUnevenTermCountList();
        }

        private void PushToAppropriateSet(double term) {
            var shouldAddToUpperTerms = term > lowerTerms.Peek();

            if (shouldAddToUpperTerms) {
                log.Verbose("Adding {@Term} to UpperTerms. Current Max of LowerTerms is {@MaxLowerTerm}", term, lowerTerms.Peek());
                upperTerms.Add(term);
            } else {
                log.Verbose("Adding {@Term} to LowerTerms. Current Max of LowerTerms is {@MaxLowerTerm}", term, lowerTerms.Peek());
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
                log.Verbose("Rebalancing Trees, LowerTerms has {@ExtraTerms} more terms than UpperTerms", lowerTerms.Count - upperTerms.Count);
                upperTerms.Add(lowerTerms.Peek());
                lowerTerms.Remove();
            }
        }

        private void RebalanceUpperTerms() {
            var hasTooManyTerms = upperTerms.Count - lowerTerms.Count >= 1;

            if (hasTooManyTerms) {
                log.Verbose("Rebalancing Trees, UpperTerms has {@ExtraTerms} more terms than LowerTerms", upperTerms.Count - lowerTerms.Count);
                lowerTerms.Add(upperTerms.Peek());
                upperTerms.Remove();
            }
        }

        private bool IsEvenTermList() {
            var totalCount = lowerTerms.Count + upperTerms.Count;
            return totalCount % 2 == 0;
        }

        private double GetMedianForUnevenTermCountList() {
            var median = lowerTerms.Peek();

            log.Debug("Calculating Median for Uneven Term Lists. Median: {@Median}, LowerTermsCount: {@LowerTermsCount}, UpperTermsCount: {@UpperTermsCount}", median, lowerTerms.Count, upperTerms.Count);

            return median;
        }

        private double GetMedianForEvenTermCountList() {
            var median = (lowerTerms.Peek() + upperTerms.Peek()) / 2.0;

            log.Debug("Calculating Median for Even Term Lists. Median: {@Median}, LowerTermsCount: {@LowerTermsCount}, UpperTermsCount: {@UpperTermsCount}", median, lowerTerms.Count, upperTerms.Count);

            return median;
        }
    }
}