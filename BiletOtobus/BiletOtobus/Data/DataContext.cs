using BiletOtobus.Models;
using Microsoft.EntityFrameworkCore;

namespace BiletOtobus.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Payment> Payment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Bus)
                .WithMany(b => b.Seats)
                .HasForeignKey(s => s.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Trip)
                .WithMany(tr => tr.Tickets)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
          .HasOne(t => t.User)
          .WithMany(u => u.Tickets)
          .HasForeignKey(t => t.UserId)
          .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
      .HasOne(t => t.Ticket)
      .WithMany(u => u.Payments)
      .HasForeignKey(t => t.TicketId)
      .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
