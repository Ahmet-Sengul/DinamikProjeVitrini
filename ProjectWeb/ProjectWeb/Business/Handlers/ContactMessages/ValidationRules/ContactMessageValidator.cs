
using Business.Handlers.ContactMessages.Commands;
using FluentValidation;

namespace Business.Handlers.ContactMessages.ValidationRules
{

    public class CreateContactMessageValidator : AbstractValidator<CreateContactMessageCommand>
    {
        public CreateContactMessageValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.UserMessage).NotEmpty();

        }
    }
    public class UpdateContactMessageValidator : AbstractValidator<UpdateContactMessageCommand>
    {
        public UpdateContactMessageValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.UserMessage).NotEmpty();

        }
    }
}