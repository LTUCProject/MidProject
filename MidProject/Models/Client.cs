using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required]
        [MaxLength(256)]
        public string AccountId { get; set; } // Foreign key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Account Account { get; set; } // Navigation property
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<ClientSubscription> ClientSubscriptions { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
