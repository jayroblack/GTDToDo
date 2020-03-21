using System.Threading.Tasks;
using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IServiceOpt<TArg, TResult>
    {
        Task<Option<TResult>> Run(TArg arg);
    }

    public interface IServiceOpt<TArg, TResult, TAltrnateOutcome>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Task<Option<TResult, TAltrnateOutcome>> Run(TArg arg);
    }
}