
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
using Business.Handlers.UserMessageses.ValidationRules;


namespace Business.Handlers.UserMessageses.Commands
{


    public class UpdateUserMessagesCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string ContactMessage { get; set; }

        public class UpdateUserMessagesCommandHandler : IRequestHandler<UpdateUserMessagesCommand, IResult>
        {
            private readonly IUserMessagesRepository _userMessagesRepository;
            private readonly IMediator _mediator;

            public UpdateUserMessagesCommandHandler(IUserMessagesRepository userMessagesRepository, IMediator mediator)
            {
                _userMessagesRepository = userMessagesRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateUserMessagesValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateUserMessagesCommand request, CancellationToken cancellationToken)
            {
                var isThereUserMessagesRecord = await _userMessagesRepository.GetAsync(u => u.Id == request.Id);


                isThereUserMessagesRecord.CreatedDate = request.CreatedDate;
                isThereUserMessagesRecord.UpdatedDate = request.UpdatedDate;
                isThereUserMessagesRecord.DeletedDate = request.DeletedDate;
                isThereUserMessagesRecord.Name = request.Name;
                isThereUserMessagesRecord.Email = request.Email;
                isThereUserMessagesRecord.Subject = request.Subject;
                isThereUserMessagesRecord.ContactMessage = request.ContactMessage;


                _userMessagesRepository.Update(isThereUserMessagesRecord);
                await _userMessagesRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

