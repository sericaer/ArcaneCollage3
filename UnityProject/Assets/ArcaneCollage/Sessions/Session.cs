using ArcaneCollage.Mods.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneCollage.Sessions
{
    class Session
    {
        public IEnumerable<Entity> entities { get; set; }
    }

    public class Entity
    {
        public List<IComponent> componnets;
    }

    public interface IComponent
    {

    }
}
