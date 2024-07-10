using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Blog: BaseEntity, IEntity
    {
        public string Header { get; set; }
        public string ImgUrl { get; set; }
        public string Content { get; set; }
    }
}
