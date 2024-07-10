
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Abouts.Queries
{
    public class GetAboutQuery : IRequest<IDataResult<About>>
    {
        public int Id { get; set; }

        public class GetAboutQueryHandler : IRequestHandler<GetAboutQuery, IDataResult<About>>
        {
            private readonly IAboutRepository _aboutRepository;
            private readonly IMediator _mediator;

            public GetAboutQueryHandler(IAboutRepository aboutRepository, IMediator mediator)
            {
                _aboutRepository = aboutRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<About>> Handle(GetAboutQuery request, CancellationToken cancellationToken)
            {
                var about = await _aboutRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<About>(about);
            }
        }
    }
}
