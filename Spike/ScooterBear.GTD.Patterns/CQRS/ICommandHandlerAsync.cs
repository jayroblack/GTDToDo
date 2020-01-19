using System.Threading.Tasks;

namespace ScooterBear.GTD.Patterns.CQRS
{
    public interface ICommandHandlerAsync<TCommand>
        where TCommand : ICommand
    {
        Task Run(TCommand command);
    }
}