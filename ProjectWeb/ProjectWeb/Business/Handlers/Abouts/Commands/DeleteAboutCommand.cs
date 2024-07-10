
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


namespace Business.Handlers.Abouts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAboutCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAboutCommandHandler : IRequestHandler<DeleteAboutCommand, IResult>
        {
            private readonly IAboutRepository _aboutRepository;
            private readonly IMediator _mediator;

            public DeleteAboutCommandHandler(IAboutRepository aboutRepository, IMediator mediator)
            {
                _aboutRepository = aboutRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAboutCommand request, CancellationToken cancellationToken)
            {
                var aboutToDelete = _aboutRepository.Get(p => p.Id == request.Id);

                _aboutRepository.Delete(aboutToDelete);
                await _aboutRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

