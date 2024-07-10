
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.UserMessageses.Queries
{

    public class GetUserMessagesesQuery : IRequest<IDataResult<IEnumerable<UserMessages>>>
    {
        public class GetUserMessagesesQueryHandler : IRequestHandler<GetUserMessagesesQuery, IDataResult<IEnumerable<UserMessages>>>
        {
            private readonly IUserMessagesRepository _userMessagesRepository;
            private readonly IMediator _mediator;

            public GetUserMessagesesQueryHandler(IUserMessagesRepository userMessagesRepository, IMediator mediator)
            {
                _userMessagesRepository = userMessagesRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserMessages>>> Handle(GetUserMessagesesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserMessages>>(await _userMessagesRepository.GetListAsync());
            }
        }
    }
}