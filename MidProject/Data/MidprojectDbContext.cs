using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MidProject.Models;
using MidProject.Constants;
namespace MidProject.Data
{
    public class MidprojectDbContext : IdentityDbContext<Account>
    {
        public MidprojectDbContext(DbContextOptions<MidprojectDbContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Charger> Chargers { get; set; }
        public DbSet<ChargingStation> ChargingStations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ChargingStationFavorite> ChargingStationFavorites { get; set; }
        public DbSet<ServiceInfoFavorite> ServiceInfoFavorites { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ServiceInfo> ServiceInfos { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<ClientSubscription> ClientSubscriptions { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Admin and Account relationship
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Account)
                .WithOne(a => a.Admin)
                .HasForeignKey<Admin>(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client and Account relationship
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Client)
                .HasForeignKey<Client>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Provider and Account relationship
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Provider)
                .HasForeignKey<Provider>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking and Client relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking and ChargingStation relationship (Updated from ServiceInfo to ChargingStation)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ChargingStation)
                .WithMany(cs => cs.Bookings)
                .HasForeignKey(b => b.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking and Vehicle relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Charger and ChargingStation relationship
            modelBuilder.Entity<Charger>()
                .HasOne(c => c.ChargingStation)
                .WithMany(cs => cs.Chargers)
                .HasForeignKey(c => c.ChargingStationId)
                .OnDelete(DeleteBehavior.Cascade);

            //// ChargingStation and Location relationship
            //modelBuilder.Entity<ChargingStation>()
            //    .HasOne(cs => cs.Location)
            //    .WithMany(l => l.ChargingStations)
            //    .HasForeignKey(cs => cs.LocationId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // ChargingStation and Provider relationship
            modelBuilder.Entity<ChargingStation>()
                .HasOne(cs => cs.Provider)
                .WithMany(p => p.ChargingStations)
                .HasForeignKey(cs => cs.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment and Account relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Account)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment and Post relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // ChargingStationFavorite and ChargingStation relationship
            modelBuilder.Entity<ChargingStationFavorite>()
                .HasOne(cs => cs.ChargingStation)
                .WithMany(csf => csf.ChargingStationFavorites)
                .HasForeignKey(cs => cs.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ChargingStationFavorite and Client relationship
            modelBuilder.Entity<ChargingStationFavorite>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ChargingStationFavorites)
                .HasForeignKey(cs => cs.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceInfoFavorite and ServiceInfo relationship
            modelBuilder.Entity<ServiceInfoFavorite>()
                .HasOne(si => si.ServiceInfo)
                .WithMany(sif => sif.ServiceInfoFavorites)
                .HasForeignKey(si => si.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceInfoFavorite and Client relationship
            modelBuilder.Entity<ServiceInfoFavorite>()
                .HasOne(si => si.Client)
                .WithMany(c => c.ServiceInfoFavorites)
                .HasForeignKey(si => si.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedback and Client relationship
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedback and ServiceInfo relationship
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.ServiceInfo)
                .WithMany(si => si.Feedbacks)
                .HasForeignKey(f => f.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaintenanceLog and ChargingStation relationship
            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(ml => ml.ChargingStation)
                .WithMany(cs => cs.MaintenanceLogs)
                .HasForeignKey(ml => ml.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification and Client relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Client)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentTransaction and Session relationship
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Session)
                .WithMany(s => s.PaymentTransactions)
                .HasForeignKey(pt => pt.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentTransaction and Client relationship
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Client)
                .WithMany(c => c.PaymentTransactions)
                .HasForeignKey(pt => pt.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post and Account relationship
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Account)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceInfo and Provider relationship
            modelBuilder.Entity<ServiceInfo>()
                .HasOne(s => s.Provider)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequest and ServiceInfo relationship
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.ServiceInfo)
                .WithMany(si => si.ServiceRequests)
                .HasForeignKey(sr => sr.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequest and Client relationship
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Client)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequest and Provider relationship
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Provider)
                .WithMany(p => p.ServiceRequests)
                .HasForeignKey(sr => sr.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Session and Client relationship
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Session and ChargingStation relationship
            modelBuilder.Entity<Session>()
                .HasOne(s => s.ChargingStation)
                .WithMany(cs => cs.Sessions)
                .HasForeignKey(s => s.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClientSubscription and Client relationship
            modelBuilder.Entity<ClientSubscription>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientSubscriptions)
                .HasForeignKey(cs => cs.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClientSubscription and SubscriptionPlan relationship
            modelBuilder.Entity<ClientSubscription>()
                .HasOne(cs => cs.SubscriptionPlan)
                .WithMany(sp => sp.ClientSubscriptions)
                .HasForeignKey(cs => cs.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequest and Vehicle relationship
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Vehicle)
                .WithMany(v => v.ServiceRequests)
                .HasForeignKey(sr => sr.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);


            // Seed roles
            seedRoles(modelBuilder);
        }
        private void seedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = RoleConstants.AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = RoleConstants.ClientRoleId,
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = RoleConstants.OwnerRoleId,
                    Name = "Owner",
                    NormalizedName = "OWNER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = RoleConstants.ServicerRoleId,
                    Name = "Servicer",
                    NormalizedName = "SERVICER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }
}