using System;
using System.Collections.Generic;
using Lab2.Domain.Entities;
using Lab2.Domain.Interfaces;

namespace Lab2.BLL
{
    public class HotelBusinessLogic : IHotelBusinessLogic
    {
        private readonly IDataRepository _dataRepository;
        private readonly ICsvReader _csvReader;

        public HotelBusinessLogic(IDataRepository dataRepository, ICsvReader csvReader)
        {
            _dataRepository = dataRepository;
            _csvReader = csvReader;
        }

        public void ProcessAndSaveData(string csvFilePath)
        {
            var records = _csvReader.ReadRecords(csvFilePath);

            var hotelsDict = new Dictionary<string, Hotel>();
            var chainsDict = new Dictionary<string, HotelChain>();
            
            foreach (var record in records)
            {
                // Chain
                if (!chainsDict.TryGetValue(record.ChainName, out var chain))
                {
                    chain = new HotelChain { ChainName = record.ChainName, ApiEndpoint = "https://api.example.com" };
                    chainsDict[record.ChainName] = chain;
                }

                // Hotel
                if (!hotelsDict.TryGetValue(record.HotelName, out var hotel))
                {
                    hotel = new Hotel
                    {
                        Name = record.HotelName,
                        StarRating = record.StarRating,
                        Description = record.Description,
                        IsActive = true,
                        HotelChain = chain,
                        Location = new Location
                        {
                            Country = record.Country,
                            City = record.City,
                            Address = record.Address,
                            Latitude = record.Latitude,
                            Longitude = record.Longitude
                        }
                    };
                    hotelsDict[record.HotelName] = hotel;
                    chain.Hotels.Add(hotel);
                }

                // Room
                var room = new Room
                {
                    RoomType = record.RoomType,
                    BasePrice = record.BasePrice,
                    MaxGuests = record.MaxGuests,
                    IsAvailableNow = true,
                    Hotel = hotel
                };
                hotel.Rooms.Add(room);

                // Review
                var review = new Review
                {
                    AuthorName = record.ReviewAuthor,
                    Text = record.ReviewText,
                    Score = record.ReviewScore,
                    DatePosted = DateTime.Now,
                    Hotel = hotel
                };
                hotel.Reviews.Add(review);
            }

            // Save to DB
            _dataRepository.EnsureDatabaseCreated();
            _dataRepository.SaveHotels(hotelsDict.Values);
            
            Console.WriteLine($"Successfully saved {hotelsDict.Count} distinct hotels from {records.Count()} CSV rows to Database!");
        }
    }
}
