
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.TeamRoles.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTeamRoleCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteTeamRoleCommandHandler : IRequestHandler<DeleteTeamRoleCommand, IResult>
        {
            private readonly ITeamRoleRepository _teamRoleRepository;
            private readonly IMediator _mediator;

            public DeleteTeamRoleCommandHandler(ITeamRoleRepository teamRoleRepository, IMediator mediator)
            {
                _teamRoleRepository = teamRoleRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteTeamRoleCommand request, CancellationToken cancellationToken)
            {
                var teamRoleToDelete = _teamRoleRepository.Get(p => p.Id == request.Id);

                _teamRoleRepository.Delete(teamRoleToDelete);
                await _teamRoleRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

