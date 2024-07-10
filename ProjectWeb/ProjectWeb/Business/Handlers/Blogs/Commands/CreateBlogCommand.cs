
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Blogs.ValidationRules;

namespace Business.Handlers.Blogs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBlogCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string ImgUrl { get; set; }
        public string Content { get; set; }


        public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, IResult>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IMediator _mediator;
            public CreateBlogCommandHandler(IBlogRepository blogRepository, IMediator mediator)
            {
                _blogRepository = blogRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateBlogValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
            {
                var isThereBlogRecord = _blogRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereBlogRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedBlog = new Blog
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Header = request.Header,
                    ImgUrl = request.ImgUrl,
                    Content = request.Content,

                };

                _blogRepository.Add(addedBlog);
                await _blogRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}