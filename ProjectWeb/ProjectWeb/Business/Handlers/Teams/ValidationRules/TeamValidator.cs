
using Business.Handlers.Teams.Commands;
using FluentValidation;

namespace Business.Handlers.Teams.ValidationRules
{

    public class CreateTeamValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
    public class UpdateTeamValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
}