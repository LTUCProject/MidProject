namespace MidProject.Models.Dto.Response
{
    public class FavoriteServiceInfoResponseDto
    {
        public int FavoriteId { get; set; }
        public int ServiceInfoId { get; set; }
        public int ClientId { get; set; }
        public ICollection<ServiceInfo> ServiceInfo { get; set; }
    }
}
