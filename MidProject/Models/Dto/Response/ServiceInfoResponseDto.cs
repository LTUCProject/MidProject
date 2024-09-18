using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;

namespace MidProject.Models.Dto.Response
{
    public class ServiceInfoResponseDto
    {
        public int ServiceInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
    }
}
