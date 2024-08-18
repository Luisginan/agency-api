using AgencyApi.TokenIssuanceModule.Models;
using AgencyApi.TokenIssuanceModule.Repos;
using Core.Utils.DB;

namespace AgencyTest.TokenIssuanceModule.Repos;

public class TokenIssuanceRepositoryTest : BaseRepoTest
{
    [Fact]
    public void GetTokenIssuanceTest()
    {
        // Arrange
        var id = NawaDao.ExecuteScalar(
            "INSERT INTO token_issuance (token, issuance_date, expiry_date, customer_id, agency_id, appointment_id) " +
            "VALUES (@token, @IssuanceDate, @expireDate, 1, 1, 1)", [
                new FieldParameter("token", "token"),
                new FieldParameter("IssuanceDate", DateTime.Now),
                new FieldParameter("expireDate", DateTime.Now),
                new FieldParameter("customer_id", 1),
                new FieldParameter("agency_id", 1),
                new FieldParameter("appointment_id", 1)
            ]);
        var tokenIssuanceRepository = new TokenIssuanceRepository(NawaDao, QueryBuilder);
        
        var tokenIssuance = tokenIssuanceRepository.GetTokenIssuance(1);
        
        // Assert
        Assert.NotNull(tokenIssuance);
        Assert.Equal("token", tokenIssuance.Token);
        Assert.Equal(1, tokenIssuance.CustomerId);
        Assert.Equal(1, tokenIssuance.AgencyId);
        Assert.Equal(1, tokenIssuance.AppointmentId);
        
        // Clean up
        NawaDao.ExecuteNonQuery("DELETE FROM token_issuance WHERE id = @id", [new FieldParameter("id", id)]);
    }

    [Fact]
    public void AddTokenIssuanceTest()
    {
        // Arrange
        var tokenIssuance = new TokenIssuance
        {
            AgencyId = 1,
            CustomerId = 1,
            AppointmentId = 1,
            IssuanceDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddDays(1),
            Token = Guid.NewGuid().ToString(),
        };
        var tokenIssuanceRepository = new TokenIssuanceRepository(NawaDao, QueryBuilder);
        
        // Act
        var id = tokenIssuanceRepository.AddTokenIssuance(tokenIssuance);
        
        // Assert
        var tokenIssuanceFromDb = NawaDao.ExecuteRow<TokenIssuance>("SELECT * FROM token_issuance WHERE id = @id", [new FieldParameter("id", id)]);
        Assert.NotNull(tokenIssuanceFromDb);
        Assert.Equal(tokenIssuance.Token, tokenIssuanceFromDb.Token);
        
        // Clean up
        NawaDao.ExecuteNonQuery("DELETE FROM token_issuance WHERE id = @id", [new FieldParameter("id", id)]);
        
    }

}