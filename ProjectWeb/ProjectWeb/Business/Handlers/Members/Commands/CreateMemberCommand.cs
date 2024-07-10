
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
using Business.Handlers.Members.ValidationRules;

namespace Business.Handlers.Members.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateMemberCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string GithubUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string WebsiteUrl { get; set; }


        public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, IResult>
        {
            private readonly IMemberRepository _memberRepository;
            private readonly IMediator _mediator;
            public CreateMemberCommandHandler(IMemberRepository memberRepository, IMediator mediator)
            {
                _memberRepository = memberRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateMemberValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
            {
                var isThereMemberRecord = _memberRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereMemberRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedMember = new Member
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Name = request.Name,
                    Surname = request.Surname,
                    Role = request.Role,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    GithubUrl = request.GithubUrl,
                    LinkedinUrl = request.LinkedinUrl,
                    WebsiteUrl = request.WebsiteUrl,

                };

                _memberRepository.Add(addedMember);
                await _memberRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}