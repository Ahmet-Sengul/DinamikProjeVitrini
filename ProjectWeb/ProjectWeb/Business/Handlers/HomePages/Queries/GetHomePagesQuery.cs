
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

namespace Business.Handlers.HomePages.Queries
{

    public class GetHomePagesQuery : IRequest<IDataResult<IEnumerable<HomePage>>>
    {
        public class GetHomePagesQueryHandler : IRequestHandler<GetHomePagesQuery, IDataResult<IEnumerable<HomePage>>>
        {
            private readonly IHomePageRepository _homePageRepository;
            private readonly IMediator _mediator;

            public GetHomePagesQueryHandler(IHomePageRepository homePageRepository, IMediator mediator)
            {
                _homePageRepository = homePageRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<HomePage>>> Handle(GetHomePagesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<HomePage>>(await _homePageRepository.GetListAsync());
            }
        }
    }
}