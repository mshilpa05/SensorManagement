using Application.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure
{
    public class SensorRepository : ISensorRepository
    {
        private readonly SensorDbContext _context;
        public SensorRepository(SensorDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Sensor>> GetAllSensorsAsync()
        {
            return await _context.Sensor.ToListAsync();
        }
    }
}
