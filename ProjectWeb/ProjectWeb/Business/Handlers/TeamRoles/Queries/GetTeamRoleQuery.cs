
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.TeamRoles.Queries
{
    public class GetTeamRoleQuery : IRequest<IDataResult<TeamRole>>
    {
        public int Id { get; set; }

        public class GetTeamRoleQueryHandler : IRequestHandler<GetTeamRoleQuery, IDataResult<TeamRole>>
        {
            private readonly ITeamRoleRepository _teamRoleRepository;
            private readonly IMediator _mediator;

            public GetTeamRoleQueryHandler(ITeamRoleRepository teamRoleRepository, IMediator mediator)
            {
                _teamRoleRepository = teamRoleRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<TeamRole>> Handle(GetTeamRoleQuery request, CancellationToken cancellationToken)
            {
                var teamRole = await _teamRoleRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<TeamRole>(teamRole);
            }
        }
    }
}
