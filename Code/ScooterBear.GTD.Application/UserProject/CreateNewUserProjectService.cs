using System;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
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
        private readonly IProfileFactory _profileFactory;

        public CreateNewUserProjectService(
            IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> query, 
            IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult> persistService,
            IKnowTheDate iKnowTheDate, 
            IProfileFactory profileFactory)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _persistService = persistService ?? throw new ArgumentNullException(nameof(persistService));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
        }

        public async Task<Option<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>> Run(CreateNewUserProjectServiceArg arg)
        {
            var userId = _profileFactory.GetCurrentProfile().UserId;

            var userProjects = await _query.Run(new GetUserProjectsQuery(userId));

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
                    _persistService.Run(new PersistNewProjectServiceArg(arg.Id, userId, arg.NewProjectName,
                        dateTimeCreated));

                return Option.Some<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                    new CreateNewUserProjectServiceResult(persistResult.Project));
            },
            async () =>
            {
                var persistResult = await 
                    _persistService.Run(new PersistNewProjectServiceArg(arg.Id, userId, arg.NewProjectName, dateTimeCreated, arg.ConsistentRead));

                return Option.Some<CreateNewUserProjectServiceResult, CreateUserProjectOutcomes>(
                    new CreateNewUserProjectServiceResult(persistResult.Project));
            });
        }
    }
}
