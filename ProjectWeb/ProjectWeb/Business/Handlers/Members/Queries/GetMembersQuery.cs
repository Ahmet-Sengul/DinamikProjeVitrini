
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

namespace Business.Handlers.Members.Queries
{

    public class GetMembersQuery : IRequest<IDataResult<IEnumerable<Member>>>
    {
        public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, IDataResult<IEnumerable<Member>>>
        {
            private readonly IMemberRepository _memberRepository;
            private readonly IMediator _mediator;

            public GetMembersQueryHandler(IMemberRepository memberRepository, IMediator mediator)
            {
                _memberRepository = memberRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            //[SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Member>>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Member>>(await _memberRepository.GetListAsync());
            }
        }
    }
}