
using Business.Handlers.HomePages.Commands;
using FluentValidation;

namespace Business.Handlers.HomePages.ValidationRules
{

    public class CreateHomePageValidator : AbstractValidator<CreateHomePageCommand>
    {
        public CreateHomePageValidator()
        {
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
    public class UpdateHomePageValidator : AbstractValidator<UpdateHomePageCommand>
    {
        public UpdateHomePageValidator()
        {
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
}