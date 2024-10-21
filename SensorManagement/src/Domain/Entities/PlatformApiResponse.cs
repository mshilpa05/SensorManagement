namespace Domain.Entities
{
    public class PlatformApiResponse
    {
        public Guid StreamId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime StoredAt { get; set; }
        public int Value { get; set; }

    }
}
