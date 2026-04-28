using System;

namespace Lab2.Domain.DTOs
{
    public class CsvHotelRecord
    {
        // Hotel
        public string HotelName { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        
        // Location
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        // Chain
        public string ChainName { get; set; }
        
        // Room
        public string RoomType { get; set; }
        public double BasePrice { get; set; }
        public int MaxGuests { get; set; }
        
        // Review
        public string ReviewAuthor { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}
