
using Business.Handlers.Tecnologies.Commands;
using FluentValidation;

namespace Business.Handlers.Tecnologies.ValidationRules
{

    public class CreateTecnologyValidator : AbstractValidator<CreateTecnologyCommand>
    {
        public CreateTecnologyValidator()
        {
            RuleFor(x => x.TecDescription).NotEmpty();
            RuleFor(x => x.TecImgUrl).NotEmpty();

        }
    }
    public class UpdateTecnologyValidator : AbstractValidator<UpdateTecnologyCommand>
    {
        public UpdateTecnologyValidator()
        {
            RuleFor(x => x.TecDescription).NotEmpty();
            RuleFor(x => x.TecImgUrl).NotEmpty();

        }
    }
}