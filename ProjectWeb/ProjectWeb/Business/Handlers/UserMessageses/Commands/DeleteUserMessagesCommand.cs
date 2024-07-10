
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


namespace Business.Handlers.UserMessageses.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserMessagesCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteUserMessagesCommandHandler : IRequestHandler<DeleteUserMessagesCommand, IResult>
        {
            private readonly IUserMessagesRepository _userMessagesRepository;
            private readonly IMediator _mediator;

            public DeleteUserMessagesCommandHandler(IUserMessagesRepository userMessagesRepository, IMediator mediator)
            {
                _userMessagesRepository = userMessagesRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteUserMessagesCommand request, CancellationToken cancellationToken)
            {
                var userMessagesToDelete = _userMessagesRepository.Get(p => p.Id == request.Id);

                _userMessagesRepository.Delete(userMessagesToDelete);
                await _userMessagesRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

