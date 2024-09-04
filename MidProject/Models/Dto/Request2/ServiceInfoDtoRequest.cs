namespace MidProject.Models.Dto.Request2
{
    public class ServiceInfoDtoRequest
    {
        public int ServiceInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
        public int ProviderId { get; set; }
    }
}
