
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.ContactMessages.Queries
{
    public class GetContactMessageQuery : IRequest<IDataResult<ContactMessage>>
    {
        public int Id { get; set; }

        public class GetContactMessageQueryHandler : IRequestHandler<GetContactMessageQuery, IDataResult<ContactMessage>>
        {
            private readonly IContactMessageRepository _contactMessageRepository;
            private readonly IMediator _mediator;

            public GetContactMessageQueryHandler(IContactMessageRepository contactMessageRepository, IMediator mediator)
            {
                _contactMessageRepository = contactMessageRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ContactMessage>> Handle(GetContactMessageQuery request, CancellationToken cancellationToken)
            {
                var contactMessage = await _contactMessageRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<ContactMessage>(contactMessage);
            }
        }
    }
}
