
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


namespace Business.Handlers.HomePages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteHomePageCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteHomePageCommandHandler : IRequestHandler<DeleteHomePageCommand, IResult>
        {
            private readonly IHomePageRepository _homePageRepository;
            private readonly IMediator _mediator;

            public DeleteHomePageCommandHandler(IHomePageRepository homePageRepository, IMediator mediator)
            {
                _homePageRepository = homePageRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteHomePageCommand request, CancellationToken cancellationToken)
            {
                var homePageToDelete = _homePageRepository.Get(p => p.Id == request.Id);

                _homePageRepository.Delete(homePageToDelete);
                await _homePageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

