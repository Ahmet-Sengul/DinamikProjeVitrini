
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Tecnologies.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTecnologyCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteTecnologyCommandHandler : IRequestHandler<DeleteTecnologyCommand, IResult>
        {
            private readonly ITecnologyRepository _tecnologyRepository;
            private readonly IMediator _mediator;

            public DeleteTecnologyCommandHandler(ITecnologyRepository tecnologyRepository, IMediator mediator)
            {
                _tecnologyRepository = tecnologyRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteTecnologyCommand request, CancellationToken cancellationToken)
            {
                var tecnologyToDelete = _tecnologyRepository.Get(p => p.Id == request.Id);

                _tecnologyRepository.Delete(tecnologyToDelete);
                await _tecnologyRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

