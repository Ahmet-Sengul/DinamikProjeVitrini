
using Business.Handlers.TeamRoles.Commands;
using FluentValidation;

namespace Business.Handlers.TeamRoles.ValidationRules
{

    public class CreateTeamRoleValidator : AbstractValidator<CreateTeamRoleCommand>
    {
        public CreateTeamRoleValidator()
        {
            RuleFor(x => x.description).NotEmpty();

        }
    }
    public class UpdateTeamRoleValidator : AbstractValidator<UpdateTeamRoleCommand>
    {
        public UpdateTeamRoleValidator()
        {
            RuleFor(x => x.description).NotEmpty();

        }
    }
}