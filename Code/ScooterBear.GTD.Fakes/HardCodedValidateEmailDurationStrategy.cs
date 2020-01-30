using System;
using ScooterBear.GTD.Application.Services.Security;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.Fakes
{
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
