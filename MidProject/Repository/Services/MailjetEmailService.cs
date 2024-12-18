﻿using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MidProject.Repository.Services
{
    public class MailjetEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly MailjetClient _client;

        public MailjetEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new MailjetClient(
                _configuration["Mailjet:ApiKey"],
                _configuration["Mailjet:SecretKey"]
            );
        }

        public async Task<bool> SendEmailAsync(string toEmail, string toName, string htmlPart)
        {
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "electrovehicleworld@gmail.com")
            .Property(Send.FromName, "EV Management System")
            .Property(Send.Subject, "Registering")
            .Property(Send.HtmlPart, "This is your virifacation code " + htmlPart)
            .Property(Send.Recipients, new JArray {
            new JObject {
                {"Email", toEmail},
                {"Name", toName}
            }
        });


            MailjetResponse response = await _client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}