namespace BiletOtobus.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string? BusNumber { get; set; }
        public int TotalSeats { get; set; }
        public List<Seat>? Seats { get; set; }
        public List<Trip>? Trips { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
    }
}
