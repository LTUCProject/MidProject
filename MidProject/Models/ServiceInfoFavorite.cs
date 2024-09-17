using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class ServiceInfoFavorite
    {
        public int ServiceInfoFavoriteId { get; set; }

        [Required]
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
