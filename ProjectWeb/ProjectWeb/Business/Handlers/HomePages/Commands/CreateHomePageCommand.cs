
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
using Business.Handlers.HomePages.ValidationRules;

namespace Business.Handlers.HomePages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateHomePageCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }


        public class CreateHomePageCommandHandler : IRequestHandler<CreateHomePageCommand, IResult>
        {
            private readonly IHomePageRepository _homePageRepository;
            private readonly IMediator _mediator;
            public CreateHomePageCommandHandler(IHomePageRepository homePageRepository, IMediator mediator)
            {
                _homePageRepository = homePageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateHomePageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateHomePageCommand request, CancellationToken cancellationToken)
            {
                var isThereHomePageRecord = _homePageRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereHomePageRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedHomePage = new HomePage
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Header = request.Header,
                    ImgUrl = request.ImgUrl,
                    Description = request.Description,

                };

                _homePageRepository.Add(addedHomePage);
                await _homePageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}