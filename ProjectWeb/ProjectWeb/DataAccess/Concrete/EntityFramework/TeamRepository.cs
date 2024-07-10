
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Collections.Generic;
using Entities.Dtos;
using Core.Entities.Concrete;
namespace DataAccess.Concrete.EntityFramework
{
    public class TeamRepository : EfEntityRepositoryBase<Team, ProjectDbContext>, ITeamRepository
    {
        private readonly ProjectDbContext _context;
        public TeamRepository(ProjectDbContext context) : base(context)
        {
            _context = context;
        }

        public List<TeamDto> GetTeamDto()
        {
            var result = from Team in _context.Teams
                         join role in _context.TeamRoles
                         on Team.RoleId equals role.Id
                         join user in _context.Users
                         on Team.UserId equals user.UserId
                         select new TeamDto
                         {
                            Name = user.FullName,
                            Role = role.RoleName,
                            Email = user.Email,
                            PhoneNumber = user.MobilePhones
                            
                         };

            return result.ToList();

        }
    }
}
