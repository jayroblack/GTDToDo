namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IQueryHandler<TReq, TResp>
    where TReq : IQuery<TResp>
    where TResp : IQueryResult
    {
        TResp Run(TReq query);
    }
}
