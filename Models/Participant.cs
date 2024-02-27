using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace L_MOBILE_MARATHON.Models
{
	public class Participant
	{
		[Key]
		public int Id { get; set; }
		[Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Second Name")]
        public string SecondName { get; set; }
        
        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName("ZIP-code")]
        public string ZipCode { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Street")]
        public string? Street { get; set; }
        [DisplayName("Birthday")]
        public DateOnly Birthday { get; set; }
        [DisplayName("Place of birth")]
        public string BirthPlace { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayName("E-mail")]
		public string Email { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [DisplayName("Alias")]
        public string? Alias { get; set; }
        [DisplayName("Condition accepted")]
        public bool Condition { get; set; } = true;
        [DisplayName("Name confidential handled")]
        public bool Confidential { get; set; } = true;
        [DisplayName("Nr of marathons until now")]
        public int NbMarathon { get; set; }
        [DisplayName("Person in case of emergencies")]
        public string EmergencyPerson { get; set; }
        [DisplayName("Mobile in case of emergencies")]
        public string EmergencyMobile { get; set; }
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }

		public ICollection<Participantsrun>? Participantsruns { get; set; }


	}
}
