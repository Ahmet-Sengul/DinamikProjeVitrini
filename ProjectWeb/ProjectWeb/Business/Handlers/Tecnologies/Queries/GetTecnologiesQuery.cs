
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

namespace Business.Handlers.Tecnologies.Queries
{

    public class GetTecnologiesQuery : IRequest<IDataResult<IEnumerable<Tecnology>>>
    {
        public class GetTecnologiesQueryHandler : IRequestHandler<GetTecnologiesQuery, IDataResult<IEnumerable<Tecnology>>>
        {
            private readonly ITecnologyRepository _tecnologyRepository;
            private readonly IMediator _mediator;

            public GetTecnologiesQueryHandler(ITecnologyRepository tecnologyRepository, IMediator mediator)
            {
                _tecnologyRepository = tecnologyRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Tecnology>>> Handle(GetTecnologiesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Tecnology>>(await _tecnologyRepository.GetListAsync());
            }
        }
    }
}