namespace BanVeXeMienDong.Models
{
    public class BusSeat
    {
        public int SeatNumber { get; set; }
        public string SeatCode { get; set; }
        public bool IsAvailable { get; set; } = true;
        public BusClass BusClass { get; set; } = BusClass.Standard;
    }
}
