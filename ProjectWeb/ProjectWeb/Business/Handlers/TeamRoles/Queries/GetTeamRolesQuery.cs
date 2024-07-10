
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

namespace Business.Handlers.TeamRoles.Queries
{

    public class GetTeamRolesQuery : IRequest<IDataResult<IEnumerable<TeamRole>>>
    {
        public class GetTeamRolesQueryHandler : IRequestHandler<GetTeamRolesQuery, IDataResult<IEnumerable<TeamRole>>>
        {
            private readonly ITeamRoleRepository _teamRoleRepository;
            private readonly IMediator _mediator;

            public GetTeamRolesQueryHandler(ITeamRoleRepository teamRoleRepository, IMediator mediator)
            {
                _teamRoleRepository = teamRoleRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<TeamRole>>> Handle(GetTeamRolesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<TeamRole>>(await _teamRoleRepository.GetListAsync());
            }
        }
    }
}