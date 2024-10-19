namespace Domain.Entities
{
    public class Sensor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DateTime CreationTime { get; private set; }
        public double UpperWarning { get; set; }
        public double LowerWarning { get; set; }

        public void Update(string? name, string? location, double upperWarning, double lowerWarning)
        {
                Name = name;
                Location = location;
                UpperWarning = upperWarning;
                LowerWarning = lowerWarning;
        }
    }
}
