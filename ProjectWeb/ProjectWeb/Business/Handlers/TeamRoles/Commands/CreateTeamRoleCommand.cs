
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
using Business.Handlers.TeamRoles.ValidationRules;

namespace Business.Handlers.TeamRoles.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTeamRoleCommand : IRequest<IResult>
    {

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string RoleName { get; set; }
        public string description { get; set; }


        public class CreateTeamRoleCommandHandler : IRequestHandler<CreateTeamRoleCommand, IResult>
        {
            private readonly ITeamRoleRepository _teamRoleRepository;
            private readonly IMediator _mediator;
            public CreateTeamRoleCommandHandler(ITeamRoleRepository teamRoleRepository, IMediator mediator)
            {
                _teamRoleRepository = teamRoleRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTeamRoleValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTeamRoleCommand request, CancellationToken cancellationToken)
            {
                var isThereTeamRoleRecord = _teamRoleRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                if (isThereTeamRoleRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTeamRole = new TeamRole
                {
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    RoleName = request.RoleName,
                    description = request.description,

                };

                _teamRoleRepository.Add(addedTeamRole);
                await _teamRoleRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}