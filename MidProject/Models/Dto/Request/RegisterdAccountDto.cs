﻿namespace MidProject.Models.Dto.Request2
{
    public class RegisterdAccountDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<string> Roles { get; set; }
    }
}