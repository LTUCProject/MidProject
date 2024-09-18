namespace MidProject.Models.Dto.Request
{
    public class UpdateSessionDto
    {
        public int EnergyConsumed { get; set; }
        public int Cost { get; set; } // Add this property if not already present in Session class
    }
}
