
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
using Business.Handlers.ContactMessages.ValidationRules;


namespace Business.Handlers.ContactMessages.Commands
{


    public class UpdateContactMessageCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string UserMessage { get; set; }

        public class UpdateContactMessageCommandHandler : IRequestHandler<UpdateContactMessageCommand, IResult>
        {
            private readonly IContactMessageRepository _contactMessageRepository;
            private readonly IMediator _mediator;

            public UpdateContactMessageCommandHandler(IContactMessageRepository contactMessageRepository, IMediator mediator)
            {
                _contactMessageRepository = contactMessageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateContactMessageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateContactMessageCommand request, CancellationToken cancellationToken)
            {
                var isThereContactMessageRecord = await _contactMessageRepository.GetAsync(u => u.Id == request.Id);


                isThereContactMessageRecord.CreatedDate = request.CreatedDate;
                isThereContactMessageRecord.UpdatedDate = request.UpdatedDate;
                isThereContactMessageRecord.DeletedDate = request.DeletedDate;
                isThereContactMessageRecord.Name = request.Name;
                isThereContactMessageRecord.Email = request.Email;
                isThereContactMessageRecord.Subject = request.Subject;
                isThereContactMessageRecord.UserMessage = request.UserMessage;


                _contactMessageRepository.Update(isThereContactMessageRecord);
                await _contactMessageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

