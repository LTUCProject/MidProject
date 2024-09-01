namespace MidProject.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public ServiceInfo ServiceInfo { get; set; } //Navigator
        public int ChargingStationId { get; set; } // Foreign key
        public ChargingStation ChargingStation { get; set; } //Navigator
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
    }
}
