
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class TeamRoleRepository : EfEntityRepositoryBase<TeamRole, ProjectDbContext>, ITeamRoleRepository
    {
        public TeamRoleRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
