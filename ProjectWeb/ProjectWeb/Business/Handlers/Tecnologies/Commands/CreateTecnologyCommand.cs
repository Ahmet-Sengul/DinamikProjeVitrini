
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
using Business.Handlers.Tecnologies.ValidationRules;

namespace Business.Handlers.Tecnologies.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTecnologyCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string TecHeader { get; set; }
        public string TecDescription { get; set; }
        public string TecImgUrl { get; set; }


        public class CreateTecnologyCommandHandler : IRequestHandler<CreateTecnologyCommand, IResult>
        {
            private readonly ITecnologyRepository _tecnologyRepository;
            private readonly IMediator _mediator;
            public CreateTecnologyCommandHandler(ITecnologyRepository tecnologyRepository, IMediator mediator)
            {
                _tecnologyRepository = tecnologyRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTecnologyValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTecnologyCommand request, CancellationToken cancellationToken)
            {
                var isThereTecnologyRecord = _tecnologyRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereTecnologyRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTecnology = new Tecnology
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    TecHeader = request.TecHeader,
                    TecDescription = request.TecDescription,
                    TecImgUrl = request.TecImgUrl,

                };

                _tecnologyRepository.Add(addedTecnology);
                await _tecnologyRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}