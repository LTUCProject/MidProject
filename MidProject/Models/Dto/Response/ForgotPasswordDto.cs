using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Response
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
