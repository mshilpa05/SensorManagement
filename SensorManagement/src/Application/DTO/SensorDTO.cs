namespace Application.DTO
{
    public class SensorDTO
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public double UpperWarning { get; set; } = Double.MaxValue;
        public double LowerWarning { get; set; } = Double.MinValue;
    }
}
