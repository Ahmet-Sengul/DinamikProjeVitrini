
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
using Business.Handlers.Teams.ValidationRules;

namespace Business.Handlers.Teams.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTeamCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }


        public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, IResult>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IMediator _mediator;
            public CreateTeamCommandHandler(ITeamRepository teamRepository, IMediator mediator)
            {
                _teamRepository = teamRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTeamValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
            {
                var isThereTeamRecord = _teamRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereTeamRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTeam = new Team
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                    ImgUrl = request.ImgUrl,
                    Description = request.Description,

                };

                _teamRepository.Add(addedTeam);
                await _teamRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}