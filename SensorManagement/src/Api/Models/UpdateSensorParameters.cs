namespace SensorManagement.src.Api.Models
{
    public class UpdateSensorParameters
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double UpperWarning { get; set; } = Double.MaxValue;
        public double LowerWarning { get; set; } = Double.MinValue;
    }
}
