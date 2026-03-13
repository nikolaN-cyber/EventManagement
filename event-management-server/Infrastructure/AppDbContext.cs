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
        public DbSet<Review> _reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventAttendance>(entity =>
            {
                
                entity.HasKey(ea => new { ea.OrganizerId, ea.EventId });
                 
                entity.HasOne(ea => ea.Organizer)
                    .WithMany(u => u.AttendingEvents)
                    .HasForeignKey(ea => ea.OrganizerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ea => ea.Event)
                    .WithMany(e => e.Attendees)
                    .HasForeignKey(ea => ea.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.EventId });

                entity.HasOne(r => r.User)
                    .WithMany(u => u.UserReviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Event)
                    .WithMany(e => e.Reviews)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(r => r.Comment).HasMaxLength(1000);

                entity.ToTable(t => t.HasCheckConstraint("CK_Review_Rating_Range", "[Rating] >= 0 AND [Rating] <= 5"));
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            });
        }
    }
}
