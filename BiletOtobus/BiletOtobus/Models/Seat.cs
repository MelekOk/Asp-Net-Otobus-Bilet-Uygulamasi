using System.Net.Sockets;

namespace BiletOtobus.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string? SeatNumber { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
