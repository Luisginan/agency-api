using AgencyApi.AgencyModule.Models;
using AgencyApi.AgencyModule.Services;
using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Repos;
using AgencyApi.AppointmentModule.Services;
using AgencyApi.CustomerModule.Models;
using AgencyApi.CustomerModule.Services;
using AgencyApi.TokenIssuanceModule.Models;
using AgencyApi.TokenIssuanceModule.Services;
using Core.CExceptions;
using Moq;

namespace AgencyTest.AppointmentModule.Services;

public class AppointmentServiceTest
{
    private readonly AppointmentService _appointmentService;
    private readonly Mock<IAppointmentRepository> _appointmentRepository;
    private readonly Mock<IAgencyService> _agencyService;
    private readonly Mock<ICustomerService> _customerService;
    private readonly Mock<IAgencySettingService> _agencySettingService;
    private readonly Mock<ITokenIssuanceService> _tokenIssuanceService;
    
    public AppointmentServiceTest()
    {
        _appointmentRepository = new Mock<IAppointmentRepository>();
        _agencySettingService = new Mock<IAgencySettingService>();
        _agencyService = new Mock<IAgencyService>();
        _customerService = new Mock<ICustomerService>();
        _tokenIssuanceService = new Mock<ITokenIssuanceService>();
        _appointmentService = new AppointmentService(_appointmentRepository.Object, 
            _agencySettingService.Object, 
            _customerService.Object, 
            _agencyService.Object, 
            _tokenIssuanceService.Object);
    }

    [Fact]
    public void GetAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            Id = 1,
            CustomerId = 1,
            AgencyId = 1,
            Date = DateTime.Today,
            Description = "Description 1",
        };
        
        _appointmentRepository.Setup(x => x.GetAppointment(1)).Returns(appointment);
        
        // Act
        var result = _appointmentService.GetAppointment(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.CustomerId);
        Assert.Equal(1, result.AgencyId);
        Assert.Equal(DateTime.Today, result.Date);
        Assert.Equal("Description 1", result.Description);
    }
    
    [Fact]
    public void AddAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            Id = 1,
            CustomerId = 1,
            AgencyId = 1,
            Date = DateTime.Today,
            Description = "Description 1",
        };
        
        var agency = new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        };
        
        var token = new TokenIssuance
        {
            Id = 1,
            Token = "Token 1",
            IssuanceDate = DateTime.Today,
            ExpiryDate = DateTime.Today,
            CustomerId = 1,
            AgencyId = 1,
            AppointmentId = 1
        };
        
        _agencyService.Setup(x => x.GetAgency(1)).Returns(agency);
        _customerService.Setup(x => x.GetCustomer(1)).Returns(new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        });
        _agencySettingService.Setup(x => x.IsHoliday(1, DateTime.Today)).Returns(false);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(5);
        _appointmentRepository.Setup(x => x.AddAppointment(appointment));
        _tokenIssuanceService.Setup(x => x.AddTokenIssuance(token));
        
        // Act
        _appointmentService.AddAppointment(appointment);
        
        // Assert
        _agencyService.Verify(x => x.GetAgency(1), Times.Exactly(2));
        _customerService.Verify(x => x.GetCustomer(1), Times.Once);
        _agencySettingService.Verify(x => x.IsHoliday(1, DateTime.Today), Times.Once);
        _appointmentRepository.Verify(x => x.AddAppointment(appointment), Times.Once);
        _tokenIssuanceService.Verify(x => x.AddTokenIssuance(It.IsAny<TokenIssuance>()), Times.Once);
    }
    
    [Fact]
    public void UpdateAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            Id = 1,
            CustomerId = 1,
            AgencyId = 1,
            Date = DateTime.Today,
            Description = "Description 1",
        };
        
        var agency = new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        };
        
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _agencyService.Setup(x => x.GetAgency(1)).Returns(agency);
        _agencySettingService.Setup(x => x.IsHoliday(1, DateTime.Today)).Returns(false);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(5);
        _appointmentRepository.Setup(x => x.UpdateAppointment(appointment));
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        
        // Act
        _appointmentService.UpdateAppointment(appointment);
        
        // Assert
        _agencyService.Verify(x => x.GetAgency(1), Times.Exactly(2));
        _agencySettingService.Verify(x => x.IsHoliday(1, DateTime.Today), Times.Once);
        _appointmentRepository.Verify(x => x.UpdateAppointment(appointment), Times.Once);
    }
    
    [Fact]
    public void DeleteAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            Id = 1,
            CustomerId = 1,
            AgencyId = 1,
            Date = DateTime.Today,
            Description = "Description 1",
        };
        
        _appointmentRepository.Setup(x => x.GetAppointment(1)).Returns(appointment);
        _appointmentRepository.Setup(x => x.DeleteAppointment(1));
        
        // Act
        _appointmentService.DeleteAppointment(1);
        
        // Assert
        _appointmentRepository.Verify(x => x.GetAppointment(1), Times.Once);
        _appointmentRepository.Verify(x => x.DeleteAppointment(1), Times.Once);
    }
    
    [Fact]
    public void GetAppointmentsByCustomerTest()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns(new List<Appointment>
        {
            new()
            {
                Id = 1,
                CustomerId = 1,
                AgencyId = 1,
                Date = DateTime.Today,
                Description = "Description 1",
            }
        });
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(1, result[0].CustomerId);
        Assert.Equal(1, result[0].AgencyId);
        Assert.Equal(DateTime.Today, result[0].Date);
        Assert.Equal("Description 1", result[0].Description);
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_CustomerNotFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_AppointmentsNotFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_CustomerNotFound_AppointmentsNotFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_CustomerNotFound_AppointmentsFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_CustomerFound_AppointmentsNotFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetAppointmentsByCustomer_CustomerFound_AppointmentsFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([
            new()
            {
                Id = 1,
                CustomerId = 1,
                AgencyId = 1,
                Date = DateTime.Today,
                Description = "Description 1",
            }
        ]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(1, result[0].CustomerId);
        Assert.Equal(1, result[0].AgencyId);
        Assert.Equal(DateTime.Today, result[0].Date);
        Assert.Equal("Description 1", result[0].Description);
    }
    
    [Fact]
    public void IsFullTest()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(10);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsFullTest_NotFull()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(5);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsFullTest_AgencyNotFound()
    {
        // Arrange
        _agencyService.Setup(x => x.GetAgency(1)).Returns((Agency?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.IsFull(1, DateTime.Today));
    }
    
    [Fact]
    public void IsFullTest_AppointmentsNotFound()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(0);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsFullTest_AgencyNotFound_AppointmentsNotFound()
    {
        // Arrange
        _agencyService.Setup(x => x.GetAgency(1)).Returns((Agency?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.IsFull(1, DateTime.Today));
    }
    
    [Fact]
    public void IsFullTest_AgencyFound_AppointmentsNotFound()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(0);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsFullTest_AgencyNotFound_AppointmentsFound()
    {
        // Arrange
        _agencyService.Setup(x => x.GetAgency(1)).Returns((Agency?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.IsFull(1, DateTime.Today));
    }
    
    [Fact]
    public void IsFullTest_AgencyFound_AppointmentsFound()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(10);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsFullTest_AgencyFound_AppointmentsFound_NotFull()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(5);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsFullTest_AgencyFound_AppointmentsFound_Full()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(10);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFullTest_MaxAppointmentZero()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(10);
        _agencySettingService.Setup(x => x.GetMaxAppointmentsPerDay(1)).Returns(0);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.IsFull(1, DateTime.Today);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void GetCountOfAppointmentsOnDateTest()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(10);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.GetCountOfAppointmentsOnDate(1, DateTime.Today);
        
        // Assert
        Assert.Equal(10, result);
    }
    
    [Fact]
    public void GetCountOfAppointmentsOnDateTest_AgencyNotFound()
    {
        // Arrange
        _agencyService.Setup(x => x.GetAgency(1)).Returns((Agency?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetCountOfAppointmentsOnDate(1, DateTime.Today));
    }
    
    [Fact]
    public void GetCountOfAppointmentsOnDateTest_AppointmentsNotFound()
    {
        // Arrange
        _appointmentRepository.Setup(x => x.GetCountOfAppointmentsOnDate(1, DateTime.Today)).Returns(0);
        _agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency
        {
            Id = 1,
            Name = "Agency 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = _appointmentService.GetCountOfAppointmentsOnDate(1, DateTime.Today);
        
        // Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void GetCountOfAppointmentsOnDateTest_AgencyNotFound_AppointmentsNotFound()
    {
        // Arrange
        _agencyService.Setup(x => x.GetAgency(1)).Returns((Agency?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetCountOfAppointmentsOnDate(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentByCustomerTest()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns(new List<Appointment>
        {
            new()
            {
                Id = 1,
                CustomerId = 1,
                AgencyId = 1,
                Date = DateTime.Today,
                Description = "Description 1",
            }
        });
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(1, result[0].CustomerId);
        Assert.Equal(1, result[0].AgencyId);
        Assert.Equal(DateTime.Today, result[0].Date);
        Assert.Equal("Description 1", result[0].Description);
    }
    
    [Fact]
    public void GetAppointmentByCustomer_CustomerNotFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentByCustomer_AppointmentsNotFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetAppointmentByCustomer_CustomerNotFound_AppointmentsNotFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentByCustomer_CustomerNotFound_AppointmentsFound()
    {
        // Arrange
        _customerService.Setup(x => x.GetCustomer(1)).Returns((Customer?)null);
        
        // Act & Assert
        Assert.Throws<RepositoryException>(() => _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today));
    }
    
    [Fact]
    public void GetAppointmentByCustomer_CustomerFound_AppointmentsNotFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetAppointmentByCustomer_CustomerFound_AppointmentsFound()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "Email 1",
            Phone = "Phone 1"
        };
        
        _customerService.Setup(x => x.GetCustomer(1)).Returns(customer);
        _appointmentRepository.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns([
            new()
            {
                Id = 1,
                CustomerId = 1,
                AgencyId = 1,
                Date = DateTime.Today,
                Description = "Description 1",
            }
        ]);
        
        // Act
        var result = _appointmentService.GetAppointmentsByCustomer(1, DateTime.Today);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(1, result[0].CustomerId);
        Assert.Equal(1, result[0].AgencyId);
        Assert.Equal(DateTime.Today, result[0].Date);
        Assert.Equal("Description 1", result[0].Description);
    }
    
   
}