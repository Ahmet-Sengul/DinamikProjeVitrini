using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SocialMedia: BaseEntity,IEntity
    {
        public string Class { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Statue { get; set; }
    }
}
