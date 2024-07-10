
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Contacts.ValidationRules;


namespace Business.Handlers.Contacts.Commands
{


    public class UpdateContactCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, IResult>
        {
            private readonly IContactRepository _contactRepository;
            private readonly IMediator _mediator;

            public UpdateContactCommandHandler(IContactRepository contactRepository, IMediator mediator)
            {
                _contactRepository = contactRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateContactValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
            {
                var isThereContactRecord = await _contactRepository.GetAsync(u => u.Id == request.Id);


                isThereContactRecord.CreatedDate = request.CreatedDate;
                isThereContactRecord.UpdatedDate = request.UpdatedDate;
                isThereContactRecord.DeletedDate = request.DeletedDate;
                isThereContactRecord.Adress = request.Adress;
                isThereContactRecord.PhoneNumber = request.PhoneNumber;
                isThereContactRecord.Email = request.Email;


                _contactRepository.Update(isThereContactRecord);
                await _contactRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

