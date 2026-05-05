using System;
using System.Collections.Generic;

namespace Lab2.Domain.Entities
{
    public class Location
    {
        public string LocationId { get; set; } = Guid.NewGuid().ToString();
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public abstract class Property
    {
        public string PropertyId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool IsActive { get; set; }
        
        public string LocationId { get; set; }
        public Location Location { get; set; }

        public virtual double GetAverageRating() => 0;
    }

    public class HotelChain
    {
        public string ChainId { get; set; } = Guid.NewGuid().ToString();
        public string ChainName { get; set; }
        public string ApiEndpoint { get; set; }

        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();

        public void SyncInventory() { }
    }

    public class Hotel : Property
    {
        public int StarRating { get; set; }
        public string Description { get; set; }

        public string? ChainId { get; set; }
        public HotelChain? HotelChain { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public List<Room> CheckAvailability(DateTime start, DateTime end)
        {
            return new List<Room>();
        }

        public override double GetAverageRating()
        {
            if (Reviews == null || Reviews.Count == 0) return 0;
            double sum = 0;
            foreach (var r in Reviews) sum += r.Score;
            return sum / Reviews.Count;
        }
    }

    public class Room
    {
        public string RoomId { get; set; } = Guid.NewGuid().ToString();
        public string RoomType { get; set; }
        public double BasePrice { get; set; }
        public int MaxGuests { get; set; }
        public bool IsAvailableNow { get; set; }

        public string HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public bool LockRoom(int minutes) => true;
    }

    public class Review
    {
        public string ReviewId { get; set; } = Guid.NewGuid().ToString();
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public int Score { get; set; }
        public DateTime DatePosted { get; set; }

        public string HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
