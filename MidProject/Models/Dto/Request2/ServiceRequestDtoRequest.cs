namespace MidProject.Models.Dto.Request2
{
    public class ServiceRequestDtoRequest
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; }
        public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public string Status { get; set; }
    }
}
