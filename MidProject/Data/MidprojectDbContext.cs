using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MidProject.Models;

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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Charger> Chargers { get; set; }
        public DbSet<ChargingStation> ChargingStations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
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

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Account)
                .WithOne()
                .HasPrincipalKey<Account>(a => a.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Account)
                .WithOne()
                .HasPrincipalKey<Account>(a => a.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Account)
                .WithOne()
                .HasPrincipalKey<Account>(a => a.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ServiceInfo)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Charger>()
                .HasOne(c => c.ChargingStation)
                .WithMany(cs => cs.Chargers)
                .HasForeignKey(c => c.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChargingStation>()
                .HasOne(cs => cs.Location)
                .WithMany(l => l.ChargingStations)
                .HasForeignKey(cs => cs.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Comments)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.ServiceInfo)
                .WithMany(s => s.Favorites)
                .HasForeignKey(f => f.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.ChargingStation)
                .WithMany(cs => cs.Favorites)
                .HasForeignKey(f => f.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Client)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.ServiceInfo)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(ml => ml.ChargingStation)
                .WithMany(cs => cs.MaintenanceLogs)
                .HasForeignKey(ml => ml.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Client)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Session)
                .WithMany(s => s.PaymentTransactions)
                .HasForeignKey(pt => pt.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Client)
                .WithMany(c => c.PaymentTransactions)
                .HasForeignKey(pt => pt.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceInfo>()
                .HasOne(s => s.Provider)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.ServiceInfo)
                .WithMany(s => s.ServiceRequests)
                .HasForeignKey(sr => sr.ServiceInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Client)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Provider)
                .WithMany(p => p.ServiceRequests)
                .HasForeignKey(sr => sr.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.ChargingStation)
                .WithMany(cs => cs.Sessions)
                .HasForeignKey(s => s.ChargingStationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientSubscription>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientSubscriptions)
                .HasForeignKey(cs => cs.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientSubscription>()
                .HasOne(cs => cs.SubscriptionPlan)
                .WithMany(sp => sp.ClientSubscriptions)
                .HasForeignKey(cs => cs.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);


            seedRoles(modelBuilder, "Admin");
            seedRoles(modelBuilder, "Client");
            seedRoles(modelBuilder, "Provider");

        }
        private void seedRoles(ModelBuilder modelBuilder, string roleName)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };

            // add claims for the users
            // complete


            modelBuilder.Entity<IdentityRole>().HasData(role);

        }
        private void seedAdminUser(ModelBuilder modelBuilder)
        {
            var adminUser = new Account
            {
                Id = "admin_user_id",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };

            // Set password for the admin userr
            var passwordHasher = new PasswordHasher<Account>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "AdminPassword123!");

            modelBuilder.Entity<Account>().HasData(adminUser);

            // Assign the admin role to the admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "admin",
                UserId = adminUser.Id
            });

           
        }
    }
}