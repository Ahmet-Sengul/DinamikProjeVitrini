
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
using Business.Handlers.HomePages.ValidationRules;


namespace Business.Handlers.HomePages.Commands
{


    public class UpdateHomePageCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }

        public class UpdateHomePageCommandHandler : IRequestHandler<UpdateHomePageCommand, IResult>
        {
            private readonly IHomePageRepository _homePageRepository;
            private readonly IMediator _mediator;

            public UpdateHomePageCommandHandler(IHomePageRepository homePageRepository, IMediator mediator)
            {
                _homePageRepository = homePageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateHomePageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateHomePageCommand request, CancellationToken cancellationToken)
            {
                var isThereHomePageRecord = await _homePageRepository.GetAsync(u => u.Id == request.Id);


                isThereHomePageRecord.CreatedDate = request.CreatedDate;
                isThereHomePageRecord.UpdatedDate = request.UpdatedDate;
                isThereHomePageRecord.DeletedDate = request.DeletedDate;
                isThereHomePageRecord.Header = request.Header;
                isThereHomePageRecord.ImgUrl = request.ImgUrl;
                isThereHomePageRecord.Description = request.Description;


                _homePageRepository.Update(isThereHomePageRecord);
                await _homePageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

