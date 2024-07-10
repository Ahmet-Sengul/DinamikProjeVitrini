
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Contacts.ValidationRules;

namespace Business.Handlers.Contacts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateContactCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, IResult>
        {
            private readonly IContactRepository _contactRepository;
            private readonly IMediator _mediator;
            public CreateContactCommandHandler(IContactRepository contactRepository, IMediator mediator)
            {
                _contactRepository = contactRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateContactValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateContactCommand request, CancellationToken cancellationToken)
            {
                var isThereContactRecord = _contactRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereContactRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedContact = new Contact
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Adress = request.Adress,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,

                };

                _contactRepository.Add(addedContact);
                await _contactRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}