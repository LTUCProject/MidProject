namespace MidProject.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
    }
}
