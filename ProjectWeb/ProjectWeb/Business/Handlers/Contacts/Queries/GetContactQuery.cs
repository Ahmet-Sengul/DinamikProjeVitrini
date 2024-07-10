
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Contacts.Queries
{
    public class GetContactQuery : IRequest<IDataResult<Contact>>
    {
        public int Id { get; set; }

        public class GetContactQueryHandler : IRequestHandler<GetContactQuery, IDataResult<Contact>>
        {
            private readonly IContactRepository _contactRepository;
            private readonly IMediator _mediator;

            public GetContactQueryHandler(IContactRepository contactRepository, IMediator mediator)
            {
                _contactRepository = contactRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Contact>> Handle(GetContactQuery request, CancellationToken cancellationToken)
            {
                var contact = await _contactRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Contact>(contact);
            }
        }
    }
}
