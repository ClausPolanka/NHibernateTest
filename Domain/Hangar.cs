using System.Collections.Generic;

namespace Domain
{
    public class Hangar
    {
        public virtual string Name { get; set; }
        public virtual IList<Mechanic> Mechanics { get; set; }
        public virtual IList<Aircraft> Aircrafts { get; set; }
    }
}