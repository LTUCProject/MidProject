﻿namespace MidProject.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        //public string AccountId { get; set; } // Changed to string to match IdentityUser key type
        public Account Account { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; }
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; }
    }
}
