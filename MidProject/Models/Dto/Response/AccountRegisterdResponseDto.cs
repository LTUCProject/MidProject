namespace MidProject.Models.Dto.Response
{
    public class AccountRegisterdResponseDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
