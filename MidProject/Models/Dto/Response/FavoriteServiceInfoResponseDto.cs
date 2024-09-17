namespace MidProject.Models.Dto.Response
{
    public class FavoriteServiceInfoResponseDto
    {
        public int FavoriteId { get; set; }
        public int ServiceInfoId { get; set; }
        public int ClientId { get; set; }
        public string ServiceInfoName { get; set; } // Only necessary properties
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
    }
}
