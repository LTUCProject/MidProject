using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class ClientDto
    {
        public int ClientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

       
      
    }

}
