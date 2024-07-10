
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


namespace Business.Handlers.Teams.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTeamCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, IResult>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IMediator _mediator;

            public DeleteTeamCommandHandler(ITeamRepository teamRepository, IMediator mediator)
            {
                _teamRepository = teamRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
            {
                var teamToDelete = _teamRepository.Get(p => p.Id == request.Id);

                _teamRepository.Delete(teamToDelete);
                await _teamRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

