
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
using Business.Handlers.UserMessageses.ValidationRules;
using System;

namespace Business.Handlers.UserMessageses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserMessagesCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string ContactMessage { get; set; }


        public class CreateUserMessagesCommandHandler : IRequestHandler<CreateUserMessagesCommand, IResult>
        {
            private readonly IUserMessagesRepository _userMessagesRepository;
            private readonly IMediator _mediator;
            public CreateUserMessagesCommandHandler(IUserMessagesRepository userMessagesRepository, IMediator mediator)
            {
                _userMessagesRepository = userMessagesRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateUserMessagesValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateUserMessagesCommand request, CancellationToken cancellationToken)
            {
                var isThereUserMessagesRecord = _userMessagesRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereUserMessagesRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedUserMessages = new UserMessages
                {
                    CreatedDate = DateTime.Now,
                    
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject,
                    ContactMessage = request.ContactMessage,

                };

                _userMessagesRepository.Add(addedUserMessages);
                await _userMessagesRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}