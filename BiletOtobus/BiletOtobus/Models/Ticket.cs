namespace BiletOtobus.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int TripId { get; set; }
        public Trip? Trip { get; set; }
        public int SeatId { get; set; }
        public Seat? Seat { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}
