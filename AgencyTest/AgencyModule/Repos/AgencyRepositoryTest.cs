using Agency.AgencyModule.Repos;
using Core.Utils.DB;

namespace AgencyTest.AgencyModule.Repos;

public class AgencyRepositoryTest : BaseRepoTest
{
    [Fact]
    public void GetAgencyTest()
    {
        var id = (int) NawaDao.ExecuteScalar("INSERT INTO agency (name, address, city) VALUES (@name, @address, @city) returning id", new List<FieldParameter>
        {
            new("name", "PT. Lumrah Sejati"),
            new("address", "Jakarta"),
            new("city", "Jakarta")
        });
        
        var agencyRepository = new AgencyRepository(NawaDao, QueryBuilder);
        
        var agency = agencyRepository.GetAgency(id);
        
        Assert.NotNull(agency);
        Assert.Equal(id, agency.Id);
        Assert.Equal("PT. Lumrah Sejati", agency.Name);
        Assert.Equal("Jakarta", agency.Address);
        Assert.Equal("Jakarta", agency.City);
        
        //cleaning
        NawaDao.ExecuteNonQuery("DELETE FROM agency WHERE id = @id", new List<FieldParameter>
        {
            new("id", id)
        });
    }
}