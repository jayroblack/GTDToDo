using System.Threading.Tasks;
using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IServiceAsync<TArg, TResult>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Task<Option<TResult>> Run(TArg arg);
    }

    public interface IServiceAsyncWrappable<TArg, TResult, TException>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Task<Option<TResult, TException>> Run(TArg arg);
    }
}
