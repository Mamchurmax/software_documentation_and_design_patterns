using System.Collections.Generic;
using Lab2.Domain.Entities;
using Lab2.Domain.DTOs;

namespace Lab2.Domain.Interfaces
{
    public interface IDataRepository
    {
        void SaveHotels(IEnumerable<Hotel> hotels);
        void SaveLocations(IEnumerable<Location> locations);
        void SaveHotelChains(IEnumerable<HotelChain> chains);
        void EnsureDatabaseCreated();
        
        IEnumerable<Hotel> GetAllHotels();
        Hotel? GetHotelById(string id);
        void AddHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(string id);
    }

    public interface ICsvReader
    {
        IEnumerable<CsvHotelRecord> ReadRecords(string filePath);
    }

    public interface IHotelBusinessLogic
    {
        void ProcessAndSaveData(string csvFilePath);
        
        IEnumerable<Hotel> GetAllHotels();
        Hotel? GetHotelById(string id);
        void AddHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(string id);
    }

    public interface IPresentationLayer
    {
        void Run();
    }
}
