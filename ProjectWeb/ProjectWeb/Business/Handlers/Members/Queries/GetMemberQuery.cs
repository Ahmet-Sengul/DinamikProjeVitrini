
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Members.Queries
{
    public class GetMemberQuery : IRequest<IDataResult<Member>>
    {
        public int Id { get; set; }

        public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, IDataResult<Member>>
        {
            private readonly IMemberRepository _memberRepository;
            private readonly IMediator _mediator;

            public GetMemberQueryHandler(IMemberRepository memberRepository, IMediator mediator)
            {
                _memberRepository = memberRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Member>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
            {
                var member = await _memberRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Member>(member);
            }
        }
    }
}
