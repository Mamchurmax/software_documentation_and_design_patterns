using Microsoft.EntityFrameworkCore;
using Lab2.Domain.Entities;
using System;

namespace Lab2.DAL
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<HotelChain> HotelChains { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=hotels.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasKey(p => p.PropertyId);
            modelBuilder.Entity<Location>().HasKey(l => l.LocationId);
            modelBuilder.Entity<HotelChain>().HasKey(c => c.ChainId);
            modelBuilder.Entity<Room>().HasKey(r => r.RoomId);
            modelBuilder.Entity<Review>().HasKey(r => r.ReviewId);
            
            // Configure relationships
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Location)
                .WithMany()
                .HasForeignKey(p => p.LocationId);

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.HotelChain)
                .WithMany(c => c.Hotels)
                .HasForeignKey(h => h.ChainId)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelId);
        }
    }
}
