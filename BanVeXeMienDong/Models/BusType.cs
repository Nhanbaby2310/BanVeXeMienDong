namespace BanVeXeMienDong.Models
{
    public enum BusClass
    {
        Standard = 0,
        Premium = 1
    }

    public class BusType
    {
        public int Id { get; set; }
        public BusClass Class { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public decimal PriceMultiplier { get; set; }
        public int MaxSeats { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
