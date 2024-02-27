using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace L_MOBILE_MARATHON.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Event Name")]
        public string EventName { get; set; }
        [DisplayName("Start")]
        public DateTime Start { get; set; }
        [DisplayName("End")]
        public DateTime End { get; set; }
        [DisplayName("Maximum Participants ")]
        public int? MaximumParticipants { get; set; }
        [DisplayName("DaysBeforeTheEvent")]
		public int? DaysBeforeTheEvent { get; set; }
		[DisplayName("Status")]
		public string Status { get; set; }
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
	}
}
