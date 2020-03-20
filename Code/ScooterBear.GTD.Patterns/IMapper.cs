namespace ScooterBear.GTD.Patterns
{
    public interface IMapTo<TFromType, TToType>
    {
        TToType MapTo(TFromType input);
    }

    public interface IMapFrom<TFromType, TToType>
    {
        TFromType MapFrom(TToType input);
    }

    public interface IMapper<TFromType, TToType> : IMapTo<TFromType, TToType>, IMapFrom<TFromType, TToType>
    {
    }
}