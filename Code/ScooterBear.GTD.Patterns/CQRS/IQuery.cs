namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface IQuery<T> where T : IQueryResult
    {
    }
}