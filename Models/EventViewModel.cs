namespace L_MOBILE_MARATHON.Models
{
	public class EventViewModel
	{
		public Event Event { get; set; }
		public List<EventAttribute> EventAttribute { get; set; } = new List<EventAttribute>();
	}
}
