using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class ChargingStationFavorite
    {
        public int ChargingStationFavoriteId { get; set; }

        [Required]
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
