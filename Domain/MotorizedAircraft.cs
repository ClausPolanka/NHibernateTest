namespace Domain
{
    public class MotorizedAircraft
    {
        public virtual int Id { get; set; }
        public virtual int EnginePower { get; set; }
        public virtual Aircraft Aircraft { get; set; }
    }
}
