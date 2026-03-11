using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<User> _users => Set<User>();
        public DbSet<Event> _events => Set<Event>();
        public DbSet<EventAttendance> _eventAttendancies => Set<EventAttendance>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventAttendance>()
                .HasKey(ea => new { ea.OrganizerId, ea.EventId });

            modelBuilder.Entity<EventAttendance>()
                .HasOne(ea => ea.Organizer)
                .WithMany(u => u.AttendingEvents)
                .HasForeignKey(ea => ea.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventAttendance>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(ea => ea.EventId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            });
        }
    }
}
