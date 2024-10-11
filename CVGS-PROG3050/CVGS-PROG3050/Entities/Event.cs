namespace CVGS_PROG3050.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Name { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public ICollection<UserEvent> UserEvents { get; set; }
    }
}
