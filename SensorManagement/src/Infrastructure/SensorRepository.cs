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

        public async Task<Sensor?> GetSensorByIdAsync(Guid id)
        {
            return await _context.Sensor.FindAsync(id);
        }

        public async Task CreateSensorAsync(Sensor sensor)
        {
            await _context.Sensor.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSensorAsync(Sensor sensor)
        {
            _context.Sensor.Update(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSensorAsync(Guid id)
        {
            var book = await _context.Sensor.FindAsync(id);
            if (book != null)
            {
                _context.Sensor.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
