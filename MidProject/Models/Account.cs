using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared;

namespace MidProject.Models
{
    public class Account : IdentityUser
    {
        public Admin Admin { get; set; }
        public Client Client { get; set; }
        public Provider Provider { get; set; }
    }
}
