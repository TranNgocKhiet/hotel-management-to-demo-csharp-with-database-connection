using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DataAccessLayer
{
    public partial class MyHotelContext : DbContext
    {
        public MyHotelContext() { }

        public MyHotelContext(DbContextOptions<MyHotelContext> options) { }

        public virtual DbSet<Customer> Customers { get; set; } = null!;

        public virtual DbSet<RoomInformation> RoomInformations { get; set; } = null!;

        public virtual DbSet<RoomType> RoomTypes { get; set; } = null!;

        public virtual DbSet<BookingDetail> BookingDetails { get; set; } = null!;

        public virtual DbSet<BookingReservation> BookingReservations { get; set; } = null!;
        
        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:MyHotelDB"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomInformation>()
            .HasKey(r => r.RoomID);

            modelBuilder.Entity<BookingDetail>()
            .HasKey(b => new { b.BookingReservationID, b.RoomID, b.StartDate, b.EndDate, b.ActualPrice });

        }
    }
}
