using System.Collections.Generic;

namespace Domain
{
    public class Mechanic
    {
        public virtual int Number { get; set; }
        public virtual IList<Hangar> Hangars { get; set; }
    }
}
