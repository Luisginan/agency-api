using AgencyApi.AgencyModule.Controllers;
using AgencyApi.AgencyModule.Dto;
using AgencyApi.AgencyModule.Models;
using AgencyApi.AgencyModule.Services;
using AutoMapper;
using Core.Utils.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgencyTest.AgencyModule.Controllers;

public class AgencyControllerTest
{
    [Fact]
    public async void GetAgencyTest()
    {
        // Arrange
        var agencyService = new Mock<IAgencyService>();
        var cache = new Mock<ICache>();
        var logger = new Mock<ILogger<AgencyControllers>>();
        var dbConnection = new Mock<IConnection>();
        var mapper = new Mock<IMapper>();
        
        var agencyController = new AgencyControllers(cache.Object, logger.Object, agencyService.Object,mapper.Object, dbConnection.Object);
        var agency = new Agency
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
     
        };
        
        agencyService.Setup(x => x.GetAgency(1)).Returns(agency);
        mapper.Setup(x => x.Map<AgencyResponseDto>(agency)).Returns(new AgencyResponseDto
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
        });
        
        // Act
        var result = await agencyController.GetAgency(1);
        
        // Assert
        var viewResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsAssignableFrom<AgencyResponseDto>(viewResult.Value);
        
        Assert.NotNull(responseDto);
        Assert.Equal(1, responseDto.Id);
        Assert.Equal("AgencyApi 1", responseDto.Name);
        Assert.Equal("Address 1", responseDto.Address);
        Assert.Equal("City 1", responseDto.City);
    }
}