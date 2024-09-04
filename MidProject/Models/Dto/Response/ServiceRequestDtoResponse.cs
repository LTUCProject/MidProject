namespace MidProject.Models.Dto.Response
{
    public class ServiceRequestDtoResponse
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; }
        public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public string Status { get; set; }
    }
}
