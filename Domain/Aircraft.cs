namespace Domain
{
    public class Aircraft
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Model { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual double Wingspan { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Name, Model, Manufacturer, Wingspan);
        }
    }
}
