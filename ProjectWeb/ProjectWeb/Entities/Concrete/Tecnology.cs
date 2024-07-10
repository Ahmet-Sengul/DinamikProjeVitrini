using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Tecnology : BaseEntity, IEntity
    {
        public string TecHeader { get; set; }
        public string TecDescription { get; set; }
        public string TecImgUrl { get; set; }
    }
}
