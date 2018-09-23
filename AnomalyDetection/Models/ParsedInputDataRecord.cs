using System;

namespace AnomalyDetection.Models
{
    public class ParsedInputDataRecord : IEquatable<ParsedInputDataRecord>
    {
        public DateTime DateTime;
        public double Value;

        public ParsedInputDataRecord(DateTime dateTime, double value)
        {
            DateTime = dateTime;
            Value = value;
        }

        public bool Equals(ParsedInputDataRecord other)
        {
            return DateTime.Equals(other.DateTime) && Value.Equals(other.Value);
        }
    }
}
