
using Business.Handlers.Members.Commands;
using FluentValidation;

namespace Business.Handlers.Members.ValidationRules
{

    public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberValidator()
        {
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty();
            RuleFor(x => x.GithubUrl).NotEmpty();
            RuleFor(x => x.LinkedinUrl).NotEmpty();
            RuleFor(x => x.WebsiteUrl).NotEmpty();

        }
    }
    public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberValidator()
        {
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty();
            RuleFor(x => x.GithubUrl).NotEmpty();
            RuleFor(x => x.LinkedinUrl).NotEmpty();
            RuleFor(x => x.WebsiteUrl).NotEmpty();

        }
    }
}