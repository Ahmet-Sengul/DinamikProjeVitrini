
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ContactMessageRepository : EfEntityRepositoryBase<ContactMessage, ProjectDbContext>, IContactMessageRepository
    {
        public ContactMessageRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
