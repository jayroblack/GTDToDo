using System;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD.Application
{
    public class GuidCreateIdStrategy : ICreateIdsStrategy
    {
        public string NewId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}