using L_MOBILE_MARATHON.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace L_MOBILE_MARATHON.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<Participantsrun> Participantsruns { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventAttribute> EventAttributes { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Participantsrun>()
				.HasKey(sc => new { sc.EventAttributeFK, sc.ParticipantFK });

			modelBuilder.Entity<Participantsrun>()
				.HasOne(sc => sc.EventAttribute)
				.WithMany(s => s.Participantsruns)
				.HasForeignKey(sc => sc.EventAttributeFK);

			modelBuilder.Entity<Participantsrun>()
				.HasOne(sc => sc.Participant)
				.WithMany(c => c.Participantsruns)
				.HasForeignKey(sc => sc.ParticipantFK);

		}

	}
}
