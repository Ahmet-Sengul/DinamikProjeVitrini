
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Teams.Queries
{
    public class GetTeamQuery : IRequest<IDataResult<Team>>
    {
        public int Id { get; set; }

        public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, IDataResult<Team>>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IMediator _mediator;

            public GetTeamQueryHandler(ITeamRepository teamRepository, IMediator mediator)
            {
                _teamRepository = teamRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Team>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
            {
                var team = await _teamRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Team>(team);
            }
        }
    }
}
