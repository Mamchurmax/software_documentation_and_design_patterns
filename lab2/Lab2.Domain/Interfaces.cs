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
    }

    public interface ICsvReader
    {
        IEnumerable<CsvHotelRecord> ReadRecords(string filePath);
    }

    public interface IHotelBusinessLogic
    {
        void ProcessAndSaveData(string csvFilePath);
    }

    public interface IPresentationLayer
    {
        void Run();
    }
}
