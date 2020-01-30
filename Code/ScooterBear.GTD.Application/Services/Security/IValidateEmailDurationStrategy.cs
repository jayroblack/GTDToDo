using System;

namespace ScooterBear.GTD.Application.Services.Security
{
    public interface IValidateEmailDurationStrategy
    {
        bool IsValid(DateTime dateTimeUtc);
    }
}
