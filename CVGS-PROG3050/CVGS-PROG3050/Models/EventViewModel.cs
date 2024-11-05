using CVGS_PROG3050.Entities;

namespace CVGS_PROG3050.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public string LocationType { get; set; }
        public string? Description { get; set; }
        public decimal? EventPrice { get; set; }
        public string CurrentUserId { get; set; }
        public ICollection<UserEvent>? UserEvents { get; set; }
    }
}
