
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


namespace Business.Handlers.Contacts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteContactCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, IResult>
        {
            private readonly IContactRepository _contactRepository;
            private readonly IMediator _mediator;

            public DeleteContactCommandHandler(IContactRepository contactRepository, IMediator mediator)
            {
                _contactRepository = contactRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
            {
                var contactToDelete = _contactRepository.Get(p => p.Id == request.Id);

                _contactRepository.Delete(contactToDelete);
                await _contactRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

