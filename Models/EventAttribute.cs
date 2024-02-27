using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L_MOBILE_MARATHON.Models
{
	public class EventAttribute
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Type")]
		public string Type { get; set; }
		[ForeignKey("EventId")]
		public int EventFK { get; set; }
		public ICollection<Participantsrun> Participantsruns { get; set; }

	}
}
