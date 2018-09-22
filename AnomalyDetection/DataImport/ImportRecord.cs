using System;

namespace AnomalyDetection.DataImport
{
    public class ImportRecord : IEquatable<ImportRecord>
    {
        public DateTime DateTime;
        public double Value;

        public ImportRecord(DateTime dateTime, double value)
        {
            DateTime = dateTime;
            Value = value;
        }

        public bool Equals(ImportRecord other)
        {
            return DateTime.Equals(other.DateTime) && Value.Equals(other.Value);
        }
    }
}
