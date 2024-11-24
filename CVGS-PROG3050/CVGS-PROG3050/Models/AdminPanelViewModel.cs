using CVGS_PROG3050.Entities;

namespace CVGS_PROG3050.Models
{
    public class AdminPanelViewModel
    {
        public Event Event { get; set; } = new Event();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
