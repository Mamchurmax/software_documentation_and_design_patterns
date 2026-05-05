using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Lab2.Domain.Entities;
using Lab2.Domain.Interfaces;

namespace Lab3.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelBusinessLogic _bll;

        public HotelController(IHotelBusinessLogic bll)
        {
            _bll = bll;
        }

        // GET: Hotel
        public IActionResult Index()
        {
            var hotels = _bll.GetAllHotels().ToList();
            return View(hotels);
        }

        // GET: Hotel/Details/5
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var hotel = _bll.GetHotelById(id);
            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // GET: Hotel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hotel hotel)
        {
            // Remove validation for navigation/auto-generated properties
            ModelState.Remove("PropertyId");
            ModelState.Remove("LocationId");
            ModelState.Remove("Location");
            ModelState.Remove("Location.Country");
            ModelState.Remove("Location.City");
            ModelState.Remove("Location.Address");
            ModelState.Remove("HotelChain");

            if (ModelState.IsValid)
            {
                hotel.PropertyId = Guid.NewGuid().ToString();
                hotel.Location = new Location
                {
                    LocationId = Guid.NewGuid().ToString(),
                    Country = "Unknown",
                    City = "Unknown",
                    Address = "Unknown"
                };
                _bll.AddHotel(hotel);
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var hotel = _bll.GetHotelById(id);
            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // POST: Hotel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Hotel hotel)
        {
            if (id != hotel.PropertyId) return NotFound();

            ModelState.Remove("LocationId");
            ModelState.Remove("Location");
            ModelState.Remove("Location.Country");
            ModelState.Remove("Location.City");
            ModelState.Remove("Location.Address");
            ModelState.Remove("HotelChain");

            if (ModelState.IsValid)
            {
                var existing = _bll.GetHotelById(id);
                if (existing != null)
                {
                    existing.Name = hotel.Name;
                    existing.StarRating = hotel.StarRating;
                    existing.Description = hotel.Description;
                    existing.IsActive = hotel.IsActive;
                    _bll.UpdateHotel(existing);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var hotel = _bll.GetHotelById(id);
            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _bll.DeleteHotel(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
