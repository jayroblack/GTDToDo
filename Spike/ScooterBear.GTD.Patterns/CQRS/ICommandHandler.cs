namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Run(TCommand command);
    }
}
