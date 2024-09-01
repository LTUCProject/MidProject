namespace MidProject.Models.Dto.Request
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
