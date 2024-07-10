
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
using Business.Handlers.Teams.ValidationRules;


namespace Business.Handlers.Teams.Commands
{


    public class UpdateTeamCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }

        public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, IResult>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IMediator _mediator;

            public UpdateTeamCommandHandler(ITeamRepository teamRepository, IMediator mediator)
            {
                _teamRepository = teamRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTeamValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
            {
                var isThereTeamRecord = await _teamRepository.GetAsync(u => u.Id == request.Id);


                isThereTeamRecord.CreatedDate = request.CreatedDate;
                isThereTeamRecord.UpdatedDate = request.UpdatedDate;
                isThereTeamRecord.DeletedDate = request.DeletedDate;
                isThereTeamRecord.UserId = request.UserId;
                isThereTeamRecord.RoleId = request.RoleId;
                isThereTeamRecord.ImgUrl = request.ImgUrl;
                isThereTeamRecord.Description = request.Description;


                _teamRepository.Update(isThereTeamRecord);
                await _teamRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

