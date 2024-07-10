
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class MemberRepository : EfEntityRepositoryBase<Member, ProjectDbContext>, IMemberRepository
    {
        public MemberRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
