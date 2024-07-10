
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Blogs.ValidationRules;


namespace Business.Handlers.Blogs.Commands
{


    public class UpdateBlogCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string ImgUrl { get; set; }
        public string Content { get; set; }

        public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, IResult>
        {
            private readonly IBlogRepository _blogRepository;
            private readonly IMediator _mediator;

            public UpdateBlogCommandHandler(IBlogRepository blogRepository, IMediator mediator)
            {
                _blogRepository = blogRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateBlogValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
            {
                var isThereBlogRecord = await _blogRepository.GetAsync(u => u.Id == request.Id);


                isThereBlogRecord.CreatedDate = request.CreatedDate;
                isThereBlogRecord.UpdatedDate = request.UpdatedDate;
                isThereBlogRecord.DeletedDate = request.DeletedDate;
                isThereBlogRecord.Header = request.Header;
                isThereBlogRecord.ImgUrl = request.ImgUrl;
                isThereBlogRecord.Content = request.Content;


                _blogRepository.Update(isThereBlogRecord);
                await _blogRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

