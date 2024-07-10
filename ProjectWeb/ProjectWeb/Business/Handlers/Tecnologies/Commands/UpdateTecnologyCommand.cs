
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
using Business.Handlers.Tecnologies.ValidationRules;


namespace Business.Handlers.Tecnologies.Commands
{


    public class UpdateTecnologyCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string TecHeader { get; set; }
        public string TecDescription { get; set; }
        public string TecImgUrl { get; set; }

        public class UpdateTecnologyCommandHandler : IRequestHandler<UpdateTecnologyCommand, IResult>
        {
            private readonly ITecnologyRepository _tecnologyRepository;
            private readonly IMediator _mediator;

            public UpdateTecnologyCommandHandler(ITecnologyRepository tecnologyRepository, IMediator mediator)
            {
                _tecnologyRepository = tecnologyRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTecnologyValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTecnologyCommand request, CancellationToken cancellationToken)
            {
                var isThereTecnologyRecord = await _tecnologyRepository.GetAsync(u => u.Id == request.Id);


                isThereTecnologyRecord.CreatedDate = request.CreatedDate;
                isThereTecnologyRecord.UpdatedDate = request.UpdatedDate;
                isThereTecnologyRecord.DeletedDate = request.DeletedDate;
                isThereTecnologyRecord.TecHeader = request.TecHeader;
                isThereTecnologyRecord.TecDescription = request.TecDescription;
                isThereTecnologyRecord.TecImgUrl = request.TecImgUrl;


                _tecnologyRepository.Update(isThereTecnologyRecord);
                await _tecnologyRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

