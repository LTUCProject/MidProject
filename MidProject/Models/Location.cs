﻿using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; }
    }
}
