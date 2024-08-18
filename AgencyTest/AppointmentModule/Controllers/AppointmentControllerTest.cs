using AgencyApi.AgencyModule.Dto;
using AgencyApi.AgencyModule.Models;
using AgencyApi.AgencyModule.Services;
using AgencyApi.AppointmentModule.Controllers;
using AgencyApi.AppointmentModule.Dto;
using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Repos;
using AgencyApi.AppointmentModule.Services;
using AgencyApi.CustomerModule.DTO;
using AgencyApi.CustomerModule.Models;
using AgencyApi.CustomerModule.Services;
using AgencyApi.TokenIssuanceModule.Models;
using AgencyApi.TokenIssuanceModule.Services;
using AutoMapper;
using Core.Utils.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgencyTest.AppointmentModule.Controllers;

public class AppointmentControllerTest
{
    private readonly Mock<IAppointmentService> _appointmentService;
    private readonly AppointmentController _appointmentController;
    private readonly Mock<IMapper> mapper;
    private readonly Mock<ICache> cache;
    private readonly Mock<IAgencyService> agencyService;
    private readonly Mock<ICustomerService> customerService;
    
    public AppointmentControllerTest()
    {
        customerService = new Mock<ICustomerService>();
        agencyService = new Mock<IAgencyService>();
        var logger = new Mock<ILogger<AppointmentController>>();
        mapper = new Mock<IMapper>();
        var dbConnection = new Mock<IConnection>();
        cache = new Mock<ICache>();
        _appointmentService = new Mock<IAppointmentService>();
        
        _appointmentController = new AppointmentController(
            cache.Object,
            logger.Object, 
            _appointmentService.Object,
            mapper.Object,
            customerService.Object,
            agencyService.Object, 
            dbConnection.Object);
    }

    [Fact]
    public async void AddAppointmentTest()
    {
        // Arrange
        
        var appointment = new AppointmentRequestDto()
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = new DateTime(2021, 1, 1)
        };
        
        // Act
        var result = await _appointmentController.AddAppointment(appointment);
        
        // Assert
        Assert.IsType<OkResult>(result);
        _appointmentService.Verify(x => x.AddAppointment(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public async void GetAppointmentTest()
    {
        cache.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync((string?) null);
        _appointmentService.Setup(x => x.GetAppointment(1)).Returns(new Appointment()
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = DateTime.Today,
            Description = "Meeting"
        });
        
        agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        customerService.Setup(x => x.GetCustomer(1)).Returns(new Customer()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "john@testing.com",
            Phone = "1234567890"
        });
        
        mapper.Setup(x => x.Map<AppointmentResponseDto>(It.IsAny<Appointment>())).Returns(new AppointmentResponseDto()
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = DateTime.Today,
            Description = "Meeting"
            
        });
        
        mapper.Setup(x => x.Map<AgencyResponseDto>(It.IsAny<Agency>())).Returns(new AgencyResponseDto()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        mapper.Setup(x => x.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(new CustomerResponse()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email = "john@testing.com",
            Phone = "1234567890"
        }); 
        
        // act
        var result = await _appointmentController.GetAppointment(1);
        
        // assert
        var viewResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsAssignableFrom<AppointmentResponseDto>(viewResult.Value);
        Assert.NotNull(responseDto);
        Assert.Equal(1, responseDto.AgencyId);
        Assert.Equal(1, responseDto.CustomerId);
        Assert.Equal(DateTime.Today, responseDto.Date);
        Assert.Equal("Meeting", responseDto.Description);
        
        Assert.NotNull(responseDto.Agency);
        Assert.Equal(1, responseDto.Agency.Id);
        Assert.Equal("AgencyApi 1", responseDto.Agency.Name);
        Assert.Equal("Address 1", responseDto.Agency.Address);
        Assert.Equal("City 1", responseDto.Agency.City);
        
        Assert.NotNull(responseDto.Customer);
        Assert.Equal(1, responseDto.Customer.Id);
        Assert.Equal("Customer 1", responseDto.Customer.Name);
        Assert.Equal("Address 1", responseDto.Customer.Address);
        Assert.Equal("john@testing.com", responseDto.Customer.Email);
        Assert.Equal("1234567890", responseDto.Customer.Phone);
    }

    [Fact]
    public async void GetAppointmentsByCustomerTest()
    {
        cache.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync((string?)null);
        _appointmentService.Setup(x => x.GetAppointmentsByCustomer(1, DateTime.Today)).Returns(new List<Appointment>()
        {
            new Appointment()
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = DateTime.Today,
                Description = "Meeting"
            }
        });

        agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });

        customerService.Setup(x => x.GetCustomer(1)).Returns(new Customer()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "john@testing.com",
            Phone = "1234567890"
        });

        mapper.Setup(x => x.Map<List<AppointmentResponseDto>>(It.IsAny<List<Appointment>>())).Returns(
        [
            new()
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = DateTime.Today,
                Description = "Meeting"
            }
        ]);

        mapper.Setup(x => x.Map<AgencyResponseDto>(It.IsAny<Agency>())).Returns(new AgencyResponseDto()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });

        mapper.Setup(x => x.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(new CustomerResponse()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email = "john@testing.com",
            Phone = "1234567890"
        });

        // act
        var result = await _appointmentController.GetAppointmentsByCustomer(1, DateTime.Today);

        // assert
        var viewResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsAssignableFrom<List<AppointmentResponseDto>>(viewResult.Value);
        Assert.NotNull(responseDto);
        Assert.Single(responseDto);

        var appointment = responseDto[0];
        Assert.Equal(1, appointment.AgencyId);
        Assert.Equal(1, appointment.CustomerId);
        Assert.Equal(DateTime.Today, appointment.Date);
        Assert.Equal("Meeting", appointment.Description);

        Assert.NotNull(appointment.Agency);
        Assert.Equal(1, appointment.Agency.Id);
        Assert.Equal("AgencyApi 1", appointment.Agency.Name);
        Assert.Equal("Address 1", appointment.Agency.Address);
        Assert.Equal("City 1", appointment.Agency.City);

        Assert.NotNull(appointment.Customer);
        Assert.Equal(1, appointment.Customer.Id);
        Assert.Equal("Customer 1", appointment.Customer.Name);
        Assert.Equal("Address 1", appointment.Customer.Address);
        Assert.Equal("john@testing.com", appointment.Customer.Email);
        Assert.Equal("1234567890", appointment.Customer.Phone);
    }
    
    [Fact]
    public async void UpdateAppointmentTest()
    {
        // Arrange
        var appointment = new AppointmentRequestDto()
        {
            AgencyId = 1,
            CustomerId = 1,
            Date = new DateTime(2021, 1, 1)
        };
        
        // Act
        var result = await _appointmentController.UpdateAppointment(appointment);
        
        // Assert
        Assert.IsType<OkResult>(result);
        _appointmentService.Verify(x => x.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
    }
    
    [Fact]
    public async void DeleteAppointmentTest()
    {
        // Arrange
        // Act
        var result = await _appointmentController.DeleteAppointment(1);
        
        // Assert
        Assert.IsType<OkResult>(result);
        _appointmentService.Verify(x => x.DeleteAppointment(1), Times.Once);
    }

    [Fact]
    public async void GetAppointmentsByAgencyTest()
    {
        cache.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync((string?)null);
        _appointmentService.Setup(x => x.GetAppointments(1, DateTime.Today)).Returns(new List<Appointment>()
        {
            new Appointment()
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = DateTime.Today,
                Description = "Meeting"
            }
        });

        agencyService.Setup(x => x.GetAgency(1)).Returns(new Agency()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });

        customerService.Setup(x => x.GetCustomer(1)).Returns(new Customer()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email2 = "john@testing.com",
            Phone = "1234567890"
        });

        mapper.Setup(x => x.Map<List<AppointmentResponseDto>>(It.IsAny<List<Appointment>>())).Returns(
        [
            new()
            {
                AgencyId = 1,
                CustomerId = 1,
                Date = DateTime.Today,
                Description = "Meeting"
            }
        ]);
        
        mapper.Setup(x => x.Map<AgencyResponseDto>(It.IsAny<Agency>())).Returns(new AgencyResponseDto()
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        mapper.Setup(x => x.Map<CustomerResponse>(It.IsAny<Customer>())).Returns(new CustomerResponse()
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1",
            Email = "john@testing.com",
            Phone = "1234567890"
        });
        
        // act
        var result = await _appointmentController.GetAppointments(1, DateTime.Today);
        
        // assert
        var viewResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsAssignableFrom<List<AppointmentResponseDto>>(viewResult.Value);
        Assert.NotNull(responseDto);
        Assert.Single(responseDto);
        
        var appointment = responseDto[0];
        Assert.Equal(1, appointment.AgencyId);
        Assert.Equal(1, appointment.CustomerId);
        Assert.Equal(DateTime.Today, appointment.Date);
        Assert.Equal("Meeting", appointment.Description);
        
        Assert.NotNull(appointment.Agency);
        Assert.Equal(1, appointment.Agency.Id);
        Assert.Equal("AgencyApi 1", appointment.Agency.Name);
        Assert.Equal("Address 1", appointment.Agency.Address);
        Assert.Equal("City 1", appointment.Agency.City);
        
        Assert.NotNull(appointment.Customer);
        Assert.Equal(1, appointment.Customer.Id);
        Assert.Equal("Customer 1", appointment.Customer.Name);
        Assert.Equal("Address 1", appointment.Customer.Address);
        Assert.Equal("john@testing.com", appointment.Customer.Email);
        Assert.Equal("1234567890", appointment.Customer.Phone);
    }
}