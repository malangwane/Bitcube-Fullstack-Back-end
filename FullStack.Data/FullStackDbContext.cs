using FullStack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.Data.DbContexts
{
    public class FullStackDbContext : DbContext
    {
        public FullStackDbContext(DbContextOptions<FullStackDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FavouriteJoin>().HasKey(fav => new { fav.UserId, fav.AdvertId });

            modelBuilder.Entity<FavouriteJoin>()
                        .HasOne(f => f.User)
                        .WithMany(c => c.FavouriteJoins)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavouriteJoin>()
                        .HasOne(f => f.Advert)
                        .WithMany(c => c.FavouriteJoins)
                        .OnDelete(DeleteBehavior.Restrict);

            // seed the database with dummy data
            modelBuilder.Entity<User>().HasData
            (
                new User()
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "properproperties1@gmail.com",
                    Password = "ppAdmin1",
                    AdminRole = true,
                },
                new User()
                {
                    Id = 3,
                    FirstName = "Johan",
                    LastName = "Smit",
                    Email = "properproperties2@gmail.com",
                    Password = "ppAdmin2",
                    AdminRole = true,
                },
                new User()
                {
                    Id = 1,
                    FirstName = "Regardt",
                    LastName = "Visagie",
                    Email = "regardtvisagie@gmail.com",
                    Password = "Reg14061465",
                    AdminRole = false,
                },
                new User()
                {
                    Id = 4,
                    FirstName = "Michelle",
                    LastName = "Koorts",
                    Email = "mk@yahoo.com",
                    Password = "Koorts123",
                    AdminRole = false,
                },
                new User()
                {
                    Id = 5,
                    FirstName = "Pieter",
                    LastName = "Joubert",
                    Email = "pieterj@yhotmail.com",
                    Password = "Jouba1987",
                    AdminRole = false,
                },
                new User()
                {
                    Id = 6,
                    FirstName = "Chulu",
                    LastName = "Sibuzo",
                    Email = "cs@ymail.com",
                    Password = "Chulu1982",
                    AdminRole = false,
                }
            );

            modelBuilder.Entity<Advert>().HasData
            (
                new Advert()
                {
                    Id = 1,
                    Header = "2 Bedroom Luxury Apartment",
                    Description = "Cozy and luxurious apartment ideal for newlyweds",
                    ProvinceId = 5,
                    CityId = 10,
                    Price = 1320000M,
                    Date = new DateTime(2020, 11, 05),
                    State = "Live",
                    Featured = true,
                    UserId = 1
                },
                new Advert()
                {
                    Id = 2,
                    Header = "Large family house that sleeps 6",
                    Description = "Has a big living room and nice view of the city...",
                    ProvinceId = 2,
                    CityId = 3,
                    Price = 2000000M,
                    Date = new DateTime(2021, 02, 25),
                    State = "Live",
                    Featured = false,
                    UserId = 1
                },
                new Advert()
                {
                    Id = 3,
                    Header = "Mansion fit for a king",
                    Description = "King Louis IV used to live here",
                    ProvinceId = 3,
                    CityId = 6,
                    Price = 11450000M,
                    Date = new DateTime(2021, 03, 03),
                    State = "Hidden",
                    Featured = false,
                    UserId = 1
                },
                new Advert()
                {
                    Id = 4,
                    Header = "Double story, 5 bedroom house with granny flat",
                    Description = "Also includes a big garden for those who love gardening",
                    ProvinceId = 5,
                    CityId = 9,
                    Price = 4500000M,
                    Date = new DateTime(2021, 03, 01),
                    State = "Live",
                    Featured = true,
                    UserId = 4
                },
                new Advert()
                {
                    Id = 5,
                    Header = "Bachelor plat ideal for students",
                    Description = "Situated in up-market area overlooking the city",
                    ProvinceId = 1,
                    CityId = 2,
                    Price = 900000M,
                    Date = new DateTime(2021, 01, 24),
                    State = "Live",
                    Featured = false,
                    UserId = 5
                },
                new Advert()
                {
                    Id = 6,
                    Header = "2 bedroom, 2 bathroom duplex",
                    Description = "Recently repainted",
                    ProvinceId = 1,
                    CityId = 2,
                    Price = 1050000M,
                    Date = new DateTime(2020, 12, 13),
                    State = "Live",
                    Featured = true,
                    UserId = 5
                },
                new Advert()
                {
                    Id = 7,
                    Header = "4 Bedroom House for Sale in Uzuli",
                    Description = "Set in one of the most secure, private and exclusive estates in Uzili Upper. High Uzili is a sought-after Security Estate of 19 architecturally designed homes with the emphasis on security, style and peace and boasts natural Fynbos gardens and private walkways.",
                    ProvinceId = 4,
                    CityId = 7,
                    Price = 5600000M,
                    Date = new DateTime(2020, 11, 07),
                    State = "Live",
                    Featured = true,
                    UserId = 6
                },
                new Advert()
                {
                    Id = 8,
                    Header = "2 Bedroom Town House for sale in Langenhovenpark",
                    Description = "This Face Brick property consists of the following: 2 Bedrooms equipped with built-in cupboards and carpets, 2 Bathroom an Open-plan lounge, dining room, and a kitchen. A private garden in a very neat condition. Double Hollywood garage.",
                    ProvinceId = 2,
                    CityId = 3,
                    Price = 860000M,
                    Date = new DateTime(2020, 03, 26),
                    State = "Live",
                    Featured = false,
                    UserId = 6
                },
                new Advert()
                {
                    Id = 9,
                    Header = "3 Bedroom Townhouse for Sale in Pellissier",
                    Description = "Spacious 205 sq and such a neat unit and complex near doctors, church and shopping center. Lots of space inside and so many cupboards. Tandem garage! Call now!!!!",
                    ProvinceId = 2,
                    CityId = 3,
                    Price = 1249000M,
                    Date = new DateTime(2020, 02, 11),
                    State = "Hidden",
                    Featured = false,
                    UserId = 6
                },
                new Advert()
                {
                    Id = 10,
                    Header = "2 Bedroom Townhouse for Sale in Groenboom",
                    Description = "Attention all young couples looking for an amazing start-up home. This beautiful two-bedroom one bathroom unit is perfect for a small family looking to settle. Situated near all major amenities you cannot ask for more, from good schools to better shopping centers this location has it all.",
                    ProvinceId = 3,
                    CityId = 5,
                    Price = 1000000M,
                    Date = new DateTime(2020, 01, 01),
                    State = "Live",
                    Featured = false,
                    UserId = 6
                }
            );

            modelBuilder.Entity<Province>().HasData
            (
                new Province()
                {
                    Id = 1,
                    Name = "Eastern Cape"
                },
                new Province()
                {
                    Id = 2,
                    Name = "Free State"
                },
                new Province()
                {
                    Id = 3,
                    Name = "Gauteng"
                },
                new Province()
                {
                    Id = 4,
                    Name = "KwaZulu-Natal"
                },
                new Province()
                {
                    Id = 5,
                    Name = "Western Cape"
                }
            );

            modelBuilder.Entity<City>().HasData
            (
                new City()
                {
                    Id = 1,
                    Name = "East London",
                    ProvinceId = 1
                },
                new City()
                {
                    Id = 2,
                    Name = "Port Elizabeth",
                    ProvinceId = 1
                },
                new City()
                {
                    Id = 3,
                    Name = "Bloemfontein",
                    ProvinceId = 2
                },
                new City()
                {
                    Id = 4,
                    Name = "Bethlehem",
                    ProvinceId = 2
                },
                new City()
                {
                    Id = 5,
                    Name = "Johannesburg",
                    ProvinceId = 3
                },
                new City()
                {
                    Id = 6,
                    Name = "Soweto",
                    ProvinceId = 3
                },
                new City()
                {
                    Id = 7,
                    Name = "Durban",
                    ProvinceId = 4
                },
                new City()
                {
                    Id = 8,
                    Name = "Pietermaritzburg",
                    ProvinceId = 4
                },
                new City()
                {
                    Id = 9,
                    Name = "Cape Town",
                    ProvinceId = 5
                },
                new City()
                {
                    Id = 10,
                    Name = "Paarl",
                    ProvinceId = 5
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}