using System;
using System.Threading.Tasks;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class CreateUserCommandHandler : ICommandHandlerAsync<CreateUserCommand>
    {
        private readonly IKnowTheDate _iKnowTheDate;

        public CreateUserCommandHandler(IKnowTheDate iKnowTheDate)
        {
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
        }
        public Task Run(CreateUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            throw new NotImplementedException();
        }
    }
}
