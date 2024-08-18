using AgencyApi.AgencyModule.Repos;
using AgencyApi.AgencyModule.Services;
using AgencyApi.AgencyModule.Models;
using Moq;

namespace AgencyTest.AgencyModule.Services;

public class AgencyServiceTest
{
    [Fact]
    public void GetAgencyTest()
    {
        // Arrange
        var agencyRepository = new Mock<IAgencyRepository>();
        var agencyService = new AgencyService(agencyRepository.Object);
        var agency = new Agency
        {
            Id = 1,
            Name = "AgencyApi 1",
            Address = "Address 1",
            City = "City 1"
     
        };
        
        agencyRepository.Setup(x => x.GetAgency(1)).Returns(agency);
        
        // Act
        var result = agencyService.GetAgency(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("AgencyApi 1", result.Name);
        Assert.Equal("Address 1", result.Address);
        Assert.Equal("City 1", result.City);
    }
}