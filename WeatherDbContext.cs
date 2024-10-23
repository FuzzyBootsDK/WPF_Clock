using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace WPF_CLock
{
    public class WeatherDbContext: DbContext
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Load the .env file
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envFilePath = System.IO.Path.Combine(appDirectory, ".env");
            string dataBasePath = System.IO.Path.Combine(appDirectory, ".db");
            Env.Load(envFilePath);

            // Get the database path from the environment variable
            string dbPath = Env.GetString("DB_PATH");

            // If the environment variable is not set, use the default path
            if (string.IsNullOrEmpty(dbPath))
            {
                dbPath = dataBasePath;
            }

            // Configure the DbContext to use SQLite with the determined path
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().ToTable("DMI Stations"); // Specify the table name using Fluent API
            modelBuilder.Entity<Location>().HasKey(l => l.stationId);
        }
    }
}
