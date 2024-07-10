
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

namespace Business.Handlers.ContactMessages.Queries
{

    public class GetContactMessagesQuery : IRequest<IDataResult<IEnumerable<ContactMessage>>>
    {
        public class GetContactMessagesQueryHandler : IRequestHandler<GetContactMessagesQuery, IDataResult<IEnumerable<ContactMessage>>>
        {
            private readonly IContactMessageRepository _contactMessageRepository;
            private readonly IMediator _mediator;

            public GetContactMessagesQueryHandler(IContactMessageRepository contactMessageRepository, IMediator mediator)
            {
                _contactMessageRepository = contactMessageRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ContactMessage>>> Handle(GetContactMessagesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<ContactMessage>>(await _contactMessageRepository.GetListAsync());
            }
        }
    }
}