namespace MidProject.Models.Dto.Response
{
    public class ServiceRequestDtoResponse
    {
        public int ServiceRequestId { get; set; }

        public int ServiceInfoId { get; set; } // Keep this field

        public int ClientId { get; set; }
        public int ProviderId { get; set; }


        public int VehicleId { get; set; } // Added VehicleId for the vehicle info

        public string Status { get; set; }
    }
}
