
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

namespace Business.Handlers.Abouts.Queries
{

    public class GetAboutsQuery : IRequest<IDataResult<IEnumerable<About>>>
    {
        public class GetAboutsQueryHandler : IRequestHandler<GetAboutsQuery, IDataResult<IEnumerable<About>>>
        {
            private readonly IAboutRepository _aboutRepository;
            private readonly IMediator _mediator;

            public GetAboutsQueryHandler(IAboutRepository aboutRepository, IMediator mediator)
            {
                _aboutRepository = aboutRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<About>>> Handle(GetAboutsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<About>>(await _aboutRepository.GetListAsync());
            }
        }
    }
}