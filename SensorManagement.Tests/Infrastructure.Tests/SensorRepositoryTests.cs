using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
    [CollectionDefinition("SequentialTests", DisableParallelization = true)]
    public class SensorRepositoryTests : IDisposable
    {
        private readonly SensorRepository _sensorRepository;
        private readonly SensorDbContext _context;
        public SensorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SensorDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new SensorDbContext(options);
            _sensorRepository = new SensorRepository(_context);

            // Clear the database at the start of each test
            _context.Sensor.RemoveRange(_context.Sensor);
            _context.SaveChanges();

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GivenNoSensorsExistInDb_OnGetAllSensorsAsync_ReturnsEmptyList()
        {
            var result = await _sensorRepository.GetAllSensorsAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GivenSensorDoesNotExistInDb_OnGetSensorByIdAsync_ReturnsNull()
        {
            var result = await _sensorRepository.GetSensorByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GivenSensorExistInDb_OnGetSensorByIdAsync_ReturnsTheSensor()
        {
            var sensor = new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };
            await _context.Sensor.AddAsync(sensor);
            await _context.SaveChangesAsync();

            var result = await _sensorRepository.GetSensorByIdAsync(sensor.Id);

            Assert.NotNull(result);
            Assert.Equal(sensor.Id, result.Id);
        }

        [Fact]
        public async Task GivenSensor_OnCreateSensorAsync_ShouldAddSensorToDatabase()
        {
            var sensor = new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };

            await _sensorRepository.CreateSensorAsync(sensor);
            var result = await _context.Sensor.FindAsync(sensor.Id);

            Assert.NotNull(result);
            Assert.Equal(sensor.Name, result?.Name);
        }

        [Fact]
        public async Task GivenSensorToUpdate_OnUpdateSensorAsync_ShouldUpdateSensor()
        {
            var sensor = new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };
            await _context.Sensor.AddAsync(sensor);
            await _context.SaveChangesAsync();
            sensor.Name = "Updated Sensor";

            await _sensorRepository.UpdateSensorAsync(sensor);
            var result = await _context.Sensor.FindAsync(sensor.Id);


            Assert.NotNull(result);
            Assert.Equal(sensor.Name, result?.Name);
            Assert.Equal(sensor.Location, result?.Location);
            Assert.Equal(sensor.CreationTime, result?.CreationTime);
            Assert.Equal(sensor.UpperWarning, result?.UpperWarning);
            Assert.Equal(sensor.LowerWarning, result?.LowerWarning);
        }

        [Fact]
        public async Task GivenSensorExistInDb_OnDeleteSensorAsync_ShouldRemoveSensorFromDb()
        {
            var sensor = new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };
            await _context.Sensor.AddAsync(sensor);
            await _context.SaveChangesAsync();

            await _sensorRepository.DeleteSensorAsync(sensor.Id);
            var result = await _sensorRepository.GetSensorByIdAsync(sensor.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GivenSensorDoesNotExistInDb_OnDeleteSensorAsync_ShouldNotRemoveAnySensorFromDb()
        {
            var sensors = new List<Sensor>
                {
                    new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 },
                    new Sensor { Id = Guid.NewGuid(), Name = "Sensor 2", Location = "Vienna", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 }
                };
            await _context.Sensor.AddAsync(sensors[0]);
            await _context.SaveChangesAsync();
            await _context.Sensor.AddAsync(sensors[1]);
            await _context.SaveChangesAsync();
            var sensorIdToDelete = Guid.NewGuid();

            await _sensorRepository.DeleteSensorAsync(sensorIdToDelete);
            var result = await _context.Sensor.ToListAsync();

            Assert.Equal(2, result.Count());
            Assert.Equal(sensors[0].Id, result.ElementAt(0).Id);
            Assert.Equal(sensors[1].Id, result.ElementAt(1).Id);
        }
    }
}
