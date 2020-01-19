using System.Threading.Tasks;
using Optional;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IQueryHandlerAsync<TReq, TResp>
        where TReq : IQuery<TResp>
        where TResp : IQueryResult
    {
        Task<Option<TResp>> Run(TReq query);
    }
}
