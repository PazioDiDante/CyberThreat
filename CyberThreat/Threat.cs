using System;

namespace CyberThreat
{
    public class Threat
    {
        public double Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Source { get; set; }
        public string Subject { get; set; }
        public bool Confidential { get; set; }
        public bool Integrity { get; set; }
        public bool Availability { get; set; }
        public string ConfidentialString { get; set; }
        public string IntegrityString { get; set; }
        public string AvailabilityString { get; set; }
        public string DateIn { get; set; }
        public string LastDate { get; set; }
        public override string ToString()
        {
            return $"УБИ.{Id} - {Name}";
        }
        public void BoolToString()
        {
            if (Confidential)
            {
                ConfidentialString = "Да";
            }
            else
            {
                ConfidentialString = "Нет";
            }
            if (Integrity)
            {
                IntegrityString = "Да";
            }
            else
            {
                IntegrityString = "Нет";
            }
            if (Availability)
            {
                AvailabilityString = "Да";
            }
            else
            {
                AvailabilityString = "Нет";
            }
        }
    }
}
