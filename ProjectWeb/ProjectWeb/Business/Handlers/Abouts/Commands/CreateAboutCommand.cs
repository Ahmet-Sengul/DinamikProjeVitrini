
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
using Business.Handlers.Abouts.ValidationRules;

namespace Business.Handlers.Abouts.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAboutCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }


        public class CreateAboutCommandHandler : IRequestHandler<CreateAboutCommand, IResult>
        {
            private readonly IAboutRepository _aboutRepository;
            private readonly IMediator _mediator;
            public CreateAboutCommandHandler(IAboutRepository aboutRepository, IMediator mediator)
            {
                _aboutRepository = aboutRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAboutValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAboutCommand request, CancellationToken cancellationToken)
            {
                var isThereAboutRecord = _aboutRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereAboutRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedAbout = new About
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Header = request.Header,
                    Content = request.Content,

                };

                _aboutRepository.Add(addedAbout);
                await _aboutRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}