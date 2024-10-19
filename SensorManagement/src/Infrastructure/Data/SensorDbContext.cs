using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SensorDbContext : DbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sensor> Sensor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>()
                .Property(sensor => sensor.CreationTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP") 
                .ValueGeneratedOnAdd();
        }
    }
}
