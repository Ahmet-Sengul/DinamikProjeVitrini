
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


namespace Business.Handlers.Members.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteMemberCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, IResult>
        {
            private readonly IMemberRepository _memberRepository;
            private readonly IMediator _mediator;

            public DeleteMemberCommandHandler(IMemberRepository memberRepository, IMediator mediator)
            {
                _memberRepository = memberRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
            {
                var memberToDelete = _memberRepository.Get(p => p.Id == request.Id);

                _memberRepository.Delete(memberToDelete);
                await _memberRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

