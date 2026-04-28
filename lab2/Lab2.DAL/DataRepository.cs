using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Lab2.Domain.Entities;
using Lab2.Domain.Interfaces;

namespace Lab2.DAL
{
    public class DataRepository : IDataRepository
    {
        private readonly HotelDbContext _context;

        public DataRepository(HotelDbContext context)
        {
            _context = context;
        }

        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void SaveHotels(IEnumerable<Hotel> hotels)
        {
            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();
        }

        public void SaveLocations(IEnumerable<Location> locations)
        {
        }

        public void SaveHotelChains(IEnumerable<HotelChain> chains)
        {
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _context.Hotels
                .Include(h => h.Location)
                .Include(h => h.HotelChain)
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .ToList();
        }

        public Hotel? GetHotelById(string id)
        {
            return _context.Hotels
                .Include(h => h.Location)
                .Include(h => h.HotelChain)
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .FirstOrDefault(h => h.PropertyId == id);
        }

        public void AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
        }

        public void UpdateHotel(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            _context.SaveChanges();
        }

        public void DeleteHotel(string id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
        }
    }
}
