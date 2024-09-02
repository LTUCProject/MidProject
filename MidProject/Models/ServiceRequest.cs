﻿namespace MidProject.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        public string Status { get; set; }
    }
}
