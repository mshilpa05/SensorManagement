namespace Domain.Entities
{
    public class Sensor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DateTime CreationTime { get; set; }
        public double UpperWarning { get; set; }
        public double LowerWarning { get; set; }
    }
}
