using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<MessageReadStatus> MessageReadStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MessageReadStatus>()
                .HasKey(x => new { x.MessageId, x.UserId });

            builder.Entity<MessageReadStatus>()
                .HasOne(x => x.User)
                .WithMany(u => u.ReadStatuses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MessageReadStatus>()
                .HasOne(x => x.Message)
                .WithMany(u => u.ReadStatuses)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
