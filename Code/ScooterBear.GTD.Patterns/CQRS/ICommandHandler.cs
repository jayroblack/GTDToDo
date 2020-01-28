using System.Threading.Tasks;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Run(TCommand command);
    }
}