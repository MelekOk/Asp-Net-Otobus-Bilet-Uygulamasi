using System.Net.Sockets;

namespace BiletOtobus.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string? DepartureCity { get; set; }
        public string? ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public double Price { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
