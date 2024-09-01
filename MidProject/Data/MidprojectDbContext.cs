using Microsoft.EntityFrameworkCore;
using MidProject.Models;

namespace MidProject.Data
{
    public class MidprojectDbContext : DbContext
    {
        public MidprojectDbContext(DbContextOptions<MidprojectDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Charger> Chargers { get; set; }
        public DbSet<ChargingStation> ChargingStations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ServiceInfo> ServiceInfos { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(u => u.User)
                .WithMany(b => b.Bookings)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
               .HasOne(s => s.ServiceInfo)
               .WithMany(b => b.Bookings)
               .HasForeignKey(s => s.ServiceInfoId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
               .HasOne(v => v.Vehicle)
               .WithMany(b => b.Bookings)
               .HasForeignKey(v => v.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Charger>()
               .HasOne(c => c.chargingStation)
               .WithMany(c => c.Chargers)
               .HasForeignKey(c => c.ChargingStationId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
             .HasOne(l => l.ChargingStation)
             .WithOne(cs => cs.Location)
             .HasForeignKey<ChargingStation>(cs => cs.LocationId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
               .WithMany(c => c.Comments)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
               .WithMany(c => c.Comments)
               .HasForeignKey(p => p.PostId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(u => u.User)
               .WithMany(f => f.Favorite)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(c => c.ChargingStation)
               .WithMany(f => f.Favorite)
               .HasForeignKey(c => c.ChargingStationId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(s => s.ServiceInfo)
               .WithMany(f => f.Favorite)
               .HasForeignKey(s => s.ServiceInfoId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(u => u.User)
               .WithMany(f => f.Feedbacks)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(s => s.ServiceInfo)
               .WithMany(f => f.Feedbacks)
               .HasForeignKey(s => s.ServiceInfoId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(c => c.ChargingStation)
               .WithMany(m => m.MaintenanceLogs)
               .HasForeignKey(c => c.ChargingStationId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                  .HasOne(u => u.User)
               .WithMany(n => n.Notifications)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(s => s.Session)
               .WithMany(p => p.PaymentTransactions)
               .HasForeignKey(s => s.SessionId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(u => u.User)
               .WithMany(p => p.PaymentTransactions)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(u => u.User)
               .WithMany(p => p.Post)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ServiceInfo>()
            //    .HasOne(s => s.serviceType)
            //   .WithMany(s => s.ServiceInfos)
            //   .HasForeignKey(s => s.ServiceTypeId)
            //   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                 .HasOne(s => s.ServiceInfo)
               .WithMany(s => s.ServiceRequests)
               .HasForeignKey(s => s.ServiceInfoId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                 .HasOne(u => u.User)
               .WithMany(s => s.ServiceRequests)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(u => u.User)
               .WithMany(s => s.Sessions)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(c => c.ChargingStation)
               .WithMany(s => s.Sessions)
               .HasForeignKey(c => c.ChargingStationId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(u => u.User)
               .WithMany(u => u.UserSubscriptions)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserSubscription>()
                 .HasOne(s => s.SubscriptionPlan)
               .WithMany(u => u.UserSubscriptions)
               .HasForeignKey(s => s.SubscriptionPlanId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasOne(u => u.User)
               .WithMany(v => v.Vehicles)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
