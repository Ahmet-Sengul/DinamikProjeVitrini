
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Tecnologies.Queries
{
    public class GetTecnologyQuery : IRequest<IDataResult<Tecnology>>
    {
        public int Id { get; set; }

        public class GetTecnologyQueryHandler : IRequestHandler<GetTecnologyQuery, IDataResult<Tecnology>>
        {
            private readonly ITecnologyRepository _tecnologyRepository;
            private readonly IMediator _mediator;

            public GetTecnologyQueryHandler(ITecnologyRepository tecnologyRepository, IMediator mediator)
            {
                _tecnologyRepository = tecnologyRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Tecnology>> Handle(GetTecnologyQuery request, CancellationToken cancellationToken)
            {
                var tecnology = await _tecnologyRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Tecnology>(tecnology);
            }
        }
    }
}
