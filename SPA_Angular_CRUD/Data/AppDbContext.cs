﻿using System.Data.Entity;
using SPA_Angular_CRUD.Models;

namespace SPA_Angular_CRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<AppDbContext>(new AppDbContextInitializer());
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}