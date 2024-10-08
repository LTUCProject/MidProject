﻿using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        public int EnergyConsumed { get; set; }
        public int Cost { get; set; }

        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
