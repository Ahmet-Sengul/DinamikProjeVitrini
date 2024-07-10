
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.UserMessageses.Queries
{
    public class GetUserMessagesQuery : IRequest<IDataResult<UserMessages>>
    {
        public int Id { get; set; }

        public class GetUserMessagesQueryHandler : IRequestHandler<GetUserMessagesQuery, IDataResult<UserMessages>>
        {
            private readonly IUserMessagesRepository _userMessagesRepository;
            private readonly IMediator _mediator;

            public GetUserMessagesQueryHandler(IUserMessagesRepository userMessagesRepository, IMediator mediator)
            {
                _userMessagesRepository = userMessagesRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserMessages>> Handle(GetUserMessagesQuery request, CancellationToken cancellationToken)
            {
                var userMessages = await _userMessagesRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<UserMessages>(userMessages);
            }
        }
    }
}
