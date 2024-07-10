
using Business.Handlers.UserMessageses.Commands;
using FluentValidation;

namespace Business.Handlers.UserMessageses.ValidationRules
{

    public class CreateUserMessagesValidator : AbstractValidator<CreateUserMessagesCommand>
    {
        public CreateUserMessagesValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.ContactMessage).NotEmpty();

        }
    }
    public class UpdateUserMessagesValidator : AbstractValidator<UpdateUserMessagesCommand>
    {
        public UpdateUserMessagesValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.ContactMessage).NotEmpty();

        }
    }
}