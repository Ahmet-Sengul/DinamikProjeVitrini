
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Teams.Queries
{

    public class GetTeamsQuery : IRequest<IDataResult<IEnumerable<Team>>>
    {
        public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IDataResult<IEnumerable<Team>>>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IMediator _mediator;

            public GetTeamsQueryHandler(ITeamRepository teamRepository, IMediator mediator)
            {
                _teamRepository = teamRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Team>>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Team>>(await _teamRepository.GetListAsync());
            }
        }
    }
}