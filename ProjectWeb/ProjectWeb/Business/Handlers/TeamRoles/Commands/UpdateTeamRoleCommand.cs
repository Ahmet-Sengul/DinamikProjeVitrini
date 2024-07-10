
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
using Business.Handlers.TeamRoles.ValidationRules;


namespace Business.Handlers.TeamRoles.Commands
{


    public class UpdateTeamRoleCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public string RoleName { get; set; }
        public string description { get; set; }

        public class UpdateTeamRoleCommandHandler : IRequestHandler<UpdateTeamRoleCommand, IResult>
        {
            private readonly ITeamRoleRepository _teamRoleRepository;
            private readonly IMediator _mediator;

            public UpdateTeamRoleCommandHandler(ITeamRoleRepository teamRoleRepository, IMediator mediator)
            {
                _teamRoleRepository = teamRoleRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTeamRoleValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTeamRoleCommand request, CancellationToken cancellationToken)
            {
                var isThereTeamRoleRecord = await _teamRoleRepository.GetAsync(u => u.Id == request.Id);


                isThereTeamRoleRecord.CreatedDate = request.CreatedDate;
                isThereTeamRoleRecord.UpdatedDate = request.UpdatedDate;
                isThereTeamRoleRecord.DeletedDate = request.DeletedDate;
                isThereTeamRoleRecord.RoleName = request.RoleName;
                isThereTeamRoleRecord.description = request.description;


                _teamRoleRepository.Update(isThereTeamRoleRecord);
                await _teamRoleRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

