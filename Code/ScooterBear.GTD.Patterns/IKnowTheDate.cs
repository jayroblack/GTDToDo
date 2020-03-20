using System;

namespace ScooterBear.GTD.Patterns
{
    public interface IKnowTheDate
    {
        DateTime Now();
        DateTime UtcNow();
    }

    public class KnowTheDate : IKnowTheDate
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}