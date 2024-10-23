using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using System.IO;

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
            Env.Load(envFilePath);

            // Get the database path from the environment variable
            string dbPath = Env.GetString("DB_PATH");

            // If the environment variable is not set, use the default path
            if (string.IsNullOrEmpty(dbPath))
            {
                dbPath = System.IO.Path.Combine(appDirectory, "Stations.db");
            }

            // Log the database path for debugging
            Debug.WriteLine($"Database path: {dbPath}");

            // Check if the database file exists
            if (!System.IO.File.Exists(dbPath))
            {
                throw new FileNotFoundException($"The database file was not found at path: {dbPath}");
            }

            // Configure the DbContext to use SQLite with the determined path
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().ToTable("DMI Stations");
            modelBuilder.Entity<Location>().HasKey(l => l.stationId);
        }
    }
}
