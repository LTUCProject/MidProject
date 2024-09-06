namespace MidProject.Models.Dto.Response
{
    public class FavoriteDtoResonse
    {
         public int FavoriteId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public int ChargingStationId { get; set; } // Foreign key
        public int ClientId { get; set; } // Foreign key
    }
}
