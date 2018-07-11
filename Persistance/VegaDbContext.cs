using Microsoft.EntityFrameworkCore;
using VehicleDealer.API.Models;

namespace VehicleDealer.API.Persistance
{
    public class VegaDbContext : DbContext
    {
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }

        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
        {

        }
    }
}