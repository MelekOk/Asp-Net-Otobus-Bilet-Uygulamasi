using BiletOtobus.Data;
using BiletOtobus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BiletOtobus.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult DatePicker()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!UserManager.IsLogin)
            {
                TempData["Message"] = "No Found";
                TempData["Status"] = "false";
                return RedirectToAction("Login");
            }

            var cities = _context.City.ToList();

            ViewBag.City = new SelectList(cities, "CityName", "CityName");

            return View();
        }

        public IActionResult Search(string fromCityName, string toCityName, DateTime date)
        {
            var fromCity = _context.City
                .Where(c => c.CityName == fromCityName)
                .Select(c => c.Id)
                .FirstOrDefault(); 
            var toCity = _context.City
                .Where(c => c.CityName == toCityName)
                .Select(c => c.Id)
                .FirstOrDefault(); 

            if (fromCity == 0 || toCity == 0)
            {
                ViewBag.Message = "Geçersiz þehir bilgileri.";
                return View("SearchResults");
            }

            var trips = _context.Trip
                .Where(t => t.DepartureCity == fromCityName &&
                            t.ArrivalCity == toCityName &&
                            t.DepartureTime.Date == date.Date)
                .ToList(); 

            ViewBag.FromCityName = fromCityName;
            ViewBag.ToCityName = toCityName;

            if (trips.Any())
            {
                return View("SearchResults", trips);
            }
            else
            {
                ViewBag.Message = "Aradýðýnýz kriterlere uygun sefer bulunamadý.";
                return View("SearchResults");
            }
        }

        [HttpGet]
        public IActionResult TripDetails(int tripId)
        {
            var trip = _context.Trip
                .FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return NotFound();
            }

            var seats = _context.Seat
                .Where(s => s.BusId == trip.BusId)
                .ToList();

            ViewBag.TripId = tripId;

            return View(seats);
        }

        [HttpPost]
        public IActionResult SelectSeats(int tripId, List<int> selectedSeatIds)
        {
            var trip =_context.Trip
                .Include(t => t.Bus)
                .FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return NotFound();
            }

            var seats = _context.Seat
                .Where(s => selectedSeatIds.Contains(s.Id))
                .ToList();

            var totalPrice = seats.Count * trip.Price;

         
                ViewBag.TripId = tripId;
                ViewBag.DepartureLocation = trip.DepartureCity;
                ViewBag.ArrivalLocation = trip.ArrivalCity;
                ViewBag.DepartureTime = trip.DepartureTime;
                ViewBag.SelectedSeats = seats;
                ViewBag.TotalPrice = totalPrice;

            return View("TicketDetails");
        }

        [HttpPost]
        public IActionResult SavedTicket(int tripId, List<int> selectedSeatIds, string name, string surname)
        {
            var trip = _context.Trip
                .Include(t => t.Bus)
                .FirstOrDefault(t => t.Id == tripId);

            if (trip == null)
            {
                return NotFound();
            }

            var seats = _context.Seat
                .Where(s => selectedSeatIds.Contains(s.Id))
                .ToList();

            var totalPrice = seats.Count * trip.Price;

            var user = new User
            {
                Name = name,
                Surname = surname
            };
            _context.User.Add(user);
            _context.SaveChanges();

            foreach (var seat in seats)
            {
                var ticket = new Ticket
                {
                    TripId = tripId,
                    SeatId = seat.Id,
                    UserId = user.Id
                };
                _context.Ticket.Add(ticket);

                seat.IsStatus = false;
            }
            _context.SaveChanges();

            ViewBag.Name = name;
            ViewBag.Surname = surname;
            ViewBag.DepartureLocation = trip.DepartureCity;
            ViewBag.ArrivalLocation = trip.ArrivalCity;
            ViewBag.DepartureTime = trip.DepartureTime;
            ViewBag.SelectedSeats = seats;
            ViewBag.TotalPrice = totalPrice;

            return View("SavedTicket");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            if(UserManager.Username == Username && UserManager.Password==Password)
            {
                UserManager.IsLogin = true;
                TempData["Message"] = "Giriþ Baþarýlý";
                TempData["Status"] = "true";
                return RedirectToAction("Index");
            }
            ViewData["Message"] = "Kullanýcý adý veya þifre hatalý.";
            ViewData["Status"] = "false";
            return View();
        }
        public IActionResult Logout()
        {
            UserManager.IsLogin = false;
            ViewData["Message"] = "Çýkýþ Baþarýlý.";
            ViewData["Status"] = "true";
            return RedirectToAction("Login");
        }

    }
}
