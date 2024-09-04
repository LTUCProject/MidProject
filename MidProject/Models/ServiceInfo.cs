﻿namespace MidProject.Models
{
    public class ServiceInfo
    {
        public int ServiceInfoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } // Added

    }
}
