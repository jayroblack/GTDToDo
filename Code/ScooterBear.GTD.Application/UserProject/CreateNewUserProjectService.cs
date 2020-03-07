using System;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public enum CreateUserProjectOutcomes
    {
        ProjectNameAlreadyExists,
        ProjectIdAlreadyExists
    }

    public class CreateNewUserProjectService : IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>
    {
        private readonly IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> _query;
        private readonly IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult> _persistService;
        private readonly IKnowTheDate _iKnowTheDate;

        public CreateNewUserProjectService(
            IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> query, 
            IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult> persistService,
            IKnowTheDate iKnowTheDate)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _persistService = persistService ?? throw new ArgumentNullException(nameof(persistService));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
        }

        public async Task<Option<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>> Run(CreateNewUserProjectServiceArg arg)
        {
            var userProjects = await _query.Run(new GetUserProjectsQuery(arg.UserId));

            var dateTimeCreated = _iKnowTheDate.UtcNow();

            return await 
                userProjects.MatchAsync(async some =>
            {
                //Project Names Must be Unique
                if (some.UserProjects.Projects.Any(x => x.Name == arg.NewProjectName && !x.IsDeleted))
                    return Option.None<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                        CreateUserProjectOutcomes.ProjectNameAlreadyExists);

                //Project Ids Must be Unique
                if( some.UserProjects.Projects.Any(x=> x.Id == arg.Id))
                    return Option.None<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                        CreateUserProjectOutcomes.ProjectIdAlreadyExists);

                var persistResult = await
                    _persistService.Run(new PersistNewProjectServiceArg(arg.Id, arg.UserId, arg.NewProjectName,
                        dateTimeCreated));

                return Option.Some<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                    new CreateNewUserProjectServiceResult(persistResult.Project));
            },
            async () =>
            {
                var persistResult = await 
                    _persistService.Run(new PersistNewProjectServiceArg(arg.Id, arg.UserId, arg.NewProjectName, dateTimeCreated));

                return Option.Some<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                    new CreateNewUserProjectServiceResult(persistResult.Project));
            });
        }
    }
}
