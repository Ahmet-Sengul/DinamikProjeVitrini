using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class TeamRole: BaseEntity, IEntity
    {
        public string RoleName { get; set; }
        public string description { get; set; }
    }
}
