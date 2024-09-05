namespace MidProject.Models.Dto.Response
{
    public class AccountLoginDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
