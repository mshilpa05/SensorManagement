using Application.DTO;
using Application.Interface;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Application.Tests
{
    public class SensorServiceTests
    {
        private readonly Mock<ISensorRepository> _mockSensorRepository;
        private readonly SensorService _sensorService;
        private readonly Mock<IMapper> _mockMapper;
        public SensorServiceTests()
        {
            _mockSensorRepository = new Mock<ISensorRepository>();
            _mockMapper = new Mock<IMapper>();
            _sensorService = new SensorService(_mockSensorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GivenListOfSensors_OnGetAllSensors_ReturnsListOfSensors()
        {
            var sensorViewDTOs = new List<SensorViewDTO>
                {
                    new SensorViewDTO { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 },
                    new SensorViewDTO { Id = Guid.NewGuid(), Name = "Sensor 2", Location = "Vienna", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 }
                };
            var sensors = new List<Sensor>
                {
                    new Sensor { Id = sensorViewDTOs[0].Id, Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 },
                    new Sensor { Id = sensorViewDTOs[1].Id, Name = "Sensor 2", Location = "Vienna", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 }
                };
            _mockSensorRepository.Setup(s => s.GetAllSensorsAsync()).ReturnsAsync(sensors);
            _mockMapper.Setup(s => s.Map<SensorViewDTO>(It.IsAny<Sensor>()))
                   .Returns((Sensor sensor) => sensorViewDTOs.First(dto => dto.Id == sensor.Id));

            var result = await _sensorService.GetAllSensorsAsync();

            Assert.NotNull(result);
            Assert.Equal(sensorViewDTOs[0].Id, result.ElementAt(0).Id);
            Assert.Equal(sensorViewDTOs[1].Id, result.ElementAt(1).Id);
        }

        [Fact]
        public async Task GivenRepositoryReturnsNull_OnGetAllSensors_ReturnNull()
        {
            _mockSensorRepository.Setup(s => s.GetAllSensorsAsync()).ReturnsAsync((IEnumerable<Sensor>?)null);

            var result = await _sensorService.GetAllSensorsAsync();

            Assert.Null(result);
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnSensorByIdAsync_ReturnNull()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync((Sensor?)null);

            var result = await _sensorService.GetSensorByIdAsync(sensorId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GivenSensorExists_OnGetSensorByIdAsync_ReturnSensorViewDTO()
        {
            var sensorId = Guid.NewGuid();
            var sensor = new Sensor { Id = sensorId, Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };
            var sensorViewDTO = new SensorViewDTO { Id = sensorId, Name = "Sensor 1", Location = "Linz", CreationTime = DateTime.UtcNow, LowerWarning = 1.0, UpperWarning = 2.0 };
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync(sensor);
            _mockMapper.Setup(m => m.Map<SensorViewDTO>(sensor)).Returns(sensorViewDTO);

            var result = await _sensorService.GetSensorByIdAsync(sensorId);

            Assert.Equal(sensorViewDTO.Id, result?.Id);
            Assert.Equal(sensorViewDTO.Name, result?.Name);
            Assert.Equal(sensorViewDTO.Location, result?.Location);
            Assert.Equal(sensorViewDTO.CreationTime, result?.CreationTime);
            Assert.Equal(sensorViewDTO.LowerWarning, result?.LowerWarning);
            Assert.Equal(sensorViewDTO.UpperWarning, result?.UpperWarning);
        }

        [Fact]
        public async Task GivenSensorIsCreatedSuccessfully_OnCreateSensorAsync_ReturnsSensorId()
        {
            var sensorCreateDTO = new SensorDTO { Name = "Sensor 1", Location = "Linz", LowerWarning = 1.0, UpperWarning = 2.0 };
            var sensor = new Sensor { Id = Guid.NewGuid(), Name = "Sensor 1", Location = "Linz", LowerWarning = 1.0, UpperWarning = 2.0, CreationTime = DateTime.UtcNow };
            _mockMapper.Setup(m => m.Map<Sensor>(sensorCreateDTO)).Returns(sensor);

            var result = await _sensorService.CreateSensorAsync(sensorCreateDTO);

            Assert.Equal(sensor.Id, result);
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnUpdateSensorAsync_ReturnFalse()
        {
            var sensorId = Guid.NewGuid();
            var sensorUpdateDTO = new SensorDTO { Name = "Sensor 1", Location = "Linz", LowerWarning = 1.0, UpperWarning = 2.0 };
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync((Sensor?)null);

            var result = await _sensorService.UpdateSensorAsync(sensorId, sensorUpdateDTO);

            Assert.False(result);
        }

        [Fact]
        public async Task GivenSensorExists_OnUpdateSensorAsync_UpdateSensorAndReturnTrue()
        {
            var sensorId = Guid.NewGuid();
            var sensor = new Sensor { Id = sensorId, Name = "Sensor 1", Location = "Linz", LowerWarning = 1.0, UpperWarning = 2.0, CreationTime = DateTime.UtcNow };
            var sensorUpdateDTO = new SensorDTO { Name = "Sensor 2", Location = "Vienna", LowerWarning = 1.0, UpperWarning = 2.0 };
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync(sensor);

            var result = await _sensorService.UpdateSensorAsync(sensorId, sensorUpdateDTO);

            Assert.True(result); 
            Assert.Equal(sensorUpdateDTO.Name, sensor.Name); 
            Assert.Equal(sensorUpdateDTO.Location, sensor.Location); 
        }

        [Fact]
        public async Task GivenSensorDoesNotExist_OnDeleteSensorAsync_ReturnFalse()
        {
            var sensorId = Guid.NewGuid();
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync((Sensor?)null);

            var result = await _sensorService.DeleteSensorAsync(sensorId);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteSensorAsync_ShouldDeleteSensorAndReturnTrue_WhenSensorExists()
        {
            var sensorId = Guid.NewGuid();
            var sensor = new Sensor { Id = sensorId, Name = "Sensor 1", Location = "Linz", LowerWarning = 1.0, UpperWarning = 2.0, CreationTime = DateTime.UtcNow };
            _mockSensorRepository.Setup(r => r.GetSensorByIdAsync(sensorId)).ReturnsAsync(sensor);

            var result = await _sensorService.DeleteSensorAsync(sensorId);

            Assert.True(result); 
        }
    }
}
