using System;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.Application.Services.Security
{
    public interface IValidateEmailDurationStrategy
    {
        bool IsValid(DateTime dateTimeUtc);
    }

    public class HardCodedValidateEmailDurationStrategy : IValidateEmailDurationStrategy
    {
        private readonly IKnowTheDate _iKkKnowTheDate;

        public HardCodedValidateEmailDurationStrategy(IKnowTheDate iKkKnowTheDate)
        {
            _iKkKnowTheDate = iKkKnowTheDate ?? throw new ArgumentNullException(nameof(iKkKnowTheDate));
        }

        public bool IsValid(DateTime dateTimeUtc)
        {
            var maxTimeSpan = TimeSpan.FromHours(1);
            var actulTimeSpan = _iKkKnowTheDate.UtcNow().Subtract(dateTimeUtc);
            return maxTimeSpan < actulTimeSpan;
        }
    }
}
