using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IService<TArg, TResult>
        where TArg : IServiceArgs<TResult>
        where TResult : IServiceResult
    {
        Option<TResult> Run(TArg arg);
    }
}
