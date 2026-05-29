namespace BanVeXeMienDong.Models
{
    public class BusInfo
    {
        public BusClass BusClass { get; set; } = BusClass.Standard;
        public int Rows { get; set; } = 10;
        public int ColsPerRow { get; set; } = 4;
        public List<BusSeat> Seats { get; set; } = new();

        public BusInfo()
        {
            InitializeSeats();
        }

        public BusInfo(BusClass busClass)
        {
            BusClass = busClass;
            if (busClass == BusClass.Premium)
            {
                Rows = 8;
                ColsPerRow = 2;
            }
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            int seatNumber = 1;
            for (int row = 1; row <= Rows; row++)
            {
                for (int col = 1; col <= ColsPerRow; col++)
                {
                    char colLetter = (char)('A' + col - 1);
                    Seats.Add(new BusSeat
                    {
                        SeatNumber = seatNumber,
                        SeatCode = $"{row}{colLetter}",
                        IsAvailable = true,
                        BusClass = BusClass
                    });
                    seatNumber++;
                }
            }
        }

        public void MarkSeatAsBooked(string seatCode)
        {
            var seat = Seats.FirstOrDefault(s => s.SeatCode == seatCode);
            if (seat != null)
            {
                seat.IsAvailable = false;
            }
        }
    }
}
