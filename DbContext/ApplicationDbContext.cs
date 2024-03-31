using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WeatherArchive.Models;

namespace WeatherArchive.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherData> WeatherData { get; set; }
    }
}
