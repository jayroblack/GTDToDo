﻿using System.Threading.Tasks;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IServiceAsync<TArg, TResult>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Task<TResult> Run(TArg arg);
    }
}
