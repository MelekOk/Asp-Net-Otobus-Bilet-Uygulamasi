namespace BiletOtobus.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
        public DateTime PaymentDate { get; set; }
        public double SumPrice { get; set;}
        public string? PaymentMethod { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
    }
}
