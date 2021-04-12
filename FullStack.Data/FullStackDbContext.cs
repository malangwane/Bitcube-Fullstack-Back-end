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
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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

          
            base.OnModelCreating(modelBuilder);
        }
    }
    
}