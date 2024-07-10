
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.HomePages.Queries
{
    public class GetHomePageQuery : IRequest<IDataResult<HomePage>>
    {
        public int Id { get; set; }

        public class GetHomePageQueryHandler : IRequestHandler<GetHomePageQuery, IDataResult<HomePage>>
        {
            private readonly IHomePageRepository _homePageRepository;
            private readonly IMediator _mediator;

            public GetHomePageQueryHandler(IHomePageRepository homePageRepository, IMediator mediator)
            {
                _homePageRepository = homePageRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<HomePage>> Handle(GetHomePageQuery request, CancellationToken cancellationToken)
            {
                var homePage = await _homePageRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<HomePage>(homePage);
            }
        }
    }
}
