using Microsoft.AspNetCore.Identity;

namespace MidProject.Models
{
    public class User 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        //public ICollection<FavoriteStation> FavoriteStations { get; set; }
        //public ICollection<FavoriteService> FavoriteServices { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
        
    }
}
