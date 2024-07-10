
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


namespace Business.Handlers.ContactMessages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteContactMessageCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, IResult>
        {
            private readonly IContactMessageRepository _contactMessageRepository;
            private readonly IMediator _mediator;

            public DeleteContactMessageCommandHandler(IContactMessageRepository contactMessageRepository, IMediator mediator)
            {
                _contactMessageRepository = contactMessageRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
            {
                var contactMessageToDelete = _contactMessageRepository.Get(p => p.Id == request.Id);

                _contactMessageRepository.Delete(contactMessageToDelete);
                await _contactMessageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

