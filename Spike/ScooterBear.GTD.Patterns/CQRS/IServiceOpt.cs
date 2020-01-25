using System.Threading.Tasks;
using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IServiceOpt<TArg, TResult>
    {
        Task<Option<TResult>> Run(TArg arg);
    }
}
