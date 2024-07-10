
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
using Business.Handlers.ContactMessages.ValidationRules;

namespace Business.Handlers.ContactMessages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateContactMessageCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string UserMessage { get; set; }


        public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, IResult>
        {
            private readonly IContactMessageRepository _contactMessageRepository;
            private readonly IMediator _mediator;
            public CreateContactMessageCommandHandler(IContactMessageRepository contactMessageRepository, IMediator mediator)
            {
                _contactMessageRepository = contactMessageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateContactMessageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
            {
                var isThereContactMessageRecord = _contactMessageRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereContactMessageRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedContactMessage = new ContactMessage
                {
                    CreatedDate = System.DateTime.Now,
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject,
                    UserMessage = request.UserMessage,

                };

                _contactMessageRepository.Add(addedContactMessage);
                await _contactMessageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}