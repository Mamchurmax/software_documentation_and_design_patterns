using System.Collections.Generic;
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
    }
}
