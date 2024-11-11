namespace CVGS_PROG3050.Models
{
    public class EventReport
    {
        public string? EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public string? LocationType { get; set; }
        public string? Description { get; set; }
        public decimal? EventPrice { get; set; }
        public int UserCount { get; set; }
    }
}
