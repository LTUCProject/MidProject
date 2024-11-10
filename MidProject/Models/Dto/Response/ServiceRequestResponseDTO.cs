namespace MidProject.Models.Dto.Response
{
    public class ServiceRequestResponseDTO
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; }
        public string ServiceInfoName { get; set; } // Include relevant fields from ServiceInfo
        public int ClientId { get; set; }
        public string ClientName { get; set; } // You can add the client's name or other details if needed
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } // Include relevant fields from Provider
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; } // Include relevant fields from Vehicle
        public string Status { get; set; }
    }
}
