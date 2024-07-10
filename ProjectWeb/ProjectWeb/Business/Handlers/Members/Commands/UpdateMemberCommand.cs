
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
using Business.Handlers.Members.ValidationRules;


namespace Business.Handlers.Members.Commands
{


    public class UpdateMemberCommand : IRequest<IResult>
    {
        public int Id { get; set; }
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

        public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, IResult>
        {
            private readonly IMemberRepository _memberRepository;
            private readonly IMediator _mediator;

            public UpdateMemberCommandHandler(IMemberRepository memberRepository, IMediator mediator)
            {
                _memberRepository = memberRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateMemberValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
            {
                var isThereMemberRecord = await _memberRepository.GetAsync(u => u.Id == request.Id);


                isThereMemberRecord.CreatedDate = request.CreatedDate;
                isThereMemberRecord.UpdatedDate = request.UpdatedDate;
                isThereMemberRecord.DeletedDate = request.DeletedDate;
                isThereMemberRecord.Name = request.Name;
                isThereMemberRecord.Surname = request.Surname;
                isThereMemberRecord.Role = request.Role;
                isThereMemberRecord.Email = request.Email;
                isThereMemberRecord.PhoneNumber = request.PhoneNumber;
                isThereMemberRecord.Description = request.Description;
                isThereMemberRecord.ImageUrl = request.ImageUrl;
                isThereMemberRecord.GithubUrl = request.GithubUrl;
                isThereMemberRecord.LinkedinUrl = request.LinkedinUrl;
                isThereMemberRecord.WebsiteUrl = request.WebsiteUrl;


                _memberRepository.Update(isThereMemberRecord);
                await _memberRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

