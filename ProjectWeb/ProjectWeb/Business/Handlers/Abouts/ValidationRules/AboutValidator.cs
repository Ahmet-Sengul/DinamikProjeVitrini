
using Business.Handlers.Abouts.Commands;
using FluentValidation;

namespace Business.Handlers.Abouts.ValidationRules
{

    public class CreateAboutValidator : AbstractValidator<CreateAboutCommand>
    {
        public CreateAboutValidator()
        {
            RuleFor(x => x.Content).NotEmpty();

        }
    }
    public class UpdateAboutValidator : AbstractValidator<UpdateAboutCommand>
    {
        public UpdateAboutValidator()
        {
            RuleFor(x => x.Content).NotEmpty();

        }
    }
}