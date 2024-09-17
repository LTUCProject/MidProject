namespace MidProject.Models.Dto.Response
{
    public class FavoriteChargingStationResponseDto
    {
        public int FavoriteId { get; set; }
        public int ChargingStationId { get; set; }
        public int ClientId { get; set; }
        public ICollection<ChargingStation> ChargingStation { get; set; }
    }
}
