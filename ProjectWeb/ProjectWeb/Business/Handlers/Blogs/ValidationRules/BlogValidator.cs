
using Business.Handlers.Blogs.Commands;
using FluentValidation;

namespace Business.Handlers.Blogs.ValidationRules
{

    public class CreateBlogValidator : AbstractValidator<CreateBlogCommand>
    {
        public CreateBlogValidator()
        {
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();

        }
    }
    public class UpdateBlogValidator : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogValidator()
        {
            RuleFor(x => x.ImgUrl).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();

        }
    }
}