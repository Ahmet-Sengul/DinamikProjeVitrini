
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class TecnologyRepository : EfEntityRepositoryBase<Tecnology, ProjectDbContext>, ITecnologyRepository
    {
        public TecnologyRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
