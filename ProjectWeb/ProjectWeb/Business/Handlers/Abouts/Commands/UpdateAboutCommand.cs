
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
using Business.Handlers.Abouts.ValidationRules;


namespace Business.Handlers.Abouts.Commands
{


    public class UpdateAboutCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }

        public class UpdateAboutCommandHandler : IRequestHandler<UpdateAboutCommand, IResult>
        {
            private readonly IAboutRepository _aboutRepository;
            private readonly IMediator _mediator;

            public UpdateAboutCommandHandler(IAboutRepository aboutRepository, IMediator mediator)
            {
                _aboutRepository = aboutRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAboutValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAboutCommand request, CancellationToken cancellationToken)
            {
                var isThereAboutRecord = await _aboutRepository.GetAsync(u => u.Id == request.Id);


                isThereAboutRecord.CreatedDate = request.CreatedDate;
                isThereAboutRecord.UpdatedDate = request.UpdatedDate;
                isThereAboutRecord.DeletedDate = request.DeletedDate;
                isThereAboutRecord.Header = request.Header;
                isThereAboutRecord.Content = request.Content;


                _aboutRepository.Update(isThereAboutRecord);
                await _aboutRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

