using System;
using System.IO;
using System.Text;
using Lab2.BLL;
using Lab2.DAL;
using Lab2.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HotelDbContext>();
builder.Services.AddTransient<IDataRepository, DataRepository>();
builder.Services.AddTransient<ICsvReader, CsvReaderService>();
builder.Services.AddTransient<IHotelBusinessLogic, HotelBusinessLogic>();

var app = builder.Build();

// Seed database with 1000 rows if empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    context.Database.EnsureCreated();

    if (!context.Hotels.Any())
    {
        Console.WriteLine("Database is empty. Seeding with 1000 CSV rows...");

        // Generate CSV
        var csvPath = Path.Combine(Path.GetTempPath(), "hotel_seed_data.csv");
        GenerateCsv(csvPath, 1000);

        // Import via BLL
        var bll = scope.ServiceProvider.GetRequiredService<IHotelBusinessLogic>();
        bll.ProcessAndSaveData(csvPath);

        // Cleanup temp file
        File.Delete(csvPath);
        Console.WriteLine("Database seeded successfully!");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

// CSV Generation (same logic as Lab2)
static void GenerateCsv(string filename, int rows)
{
    var sb = new StringBuilder();
    sb.AppendLine("HotelName,StarRating,Description,Country,City,Address,Latitude,Longitude,ChainName,RoomType,BasePrice,MaxGuests,ReviewAuthor,ReviewText,ReviewScore");

    var random = new Random();
    var chains = new[] { "Hilton", "Marriott", "Hyatt", "Radisson" };
    var cities = new[] { "Kyiv", "Lviv", "Odessa", "Kharkiv" };
    var roomTypes = new[] { "Standard", "Deluxe", "Suite", "Presidential" };

    for (int i = 1; i <= rows; i++)
    {
        var chain = chains[random.Next(chains.Length)];
        var hotelId = random.Next(1, 21);
        var hotelName = $"{chain} Hotel {hotelId}";
        var starRating = random.Next(3, 6);
        var city = cities[random.Next(cities.Length)];

        sb.AppendLine($"{hotelName},{starRating},A nice hotel,Ukraine,{city},Street {i},50.45,30.52,{chain},{roomTypes[random.Next(roomTypes.Length)]},{random.Next(50, 500)},{random.Next(1, 5)},Author {i},Good place!,{random.Next(1, 6)}");
    }
    File.WriteAllText(filename, sb.ToString());
}
