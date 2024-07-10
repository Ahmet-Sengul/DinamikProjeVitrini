using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Team: BaseEntity, IEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
    }
}
