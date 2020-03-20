﻿using System.Threading.Tasks;
using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IServiceOptOutcomes<TArg, TResult, TAltrnateOutcome>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Task<Option<TResult, TAltrnateOutcome>> Run(TArg arg);
    }
}