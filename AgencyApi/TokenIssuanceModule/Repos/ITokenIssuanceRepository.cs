using AgencyApi.TokenIssuanceModule.Models;

namespace AgencyApi.TokenIssuanceModule.Repos;

public interface ITokenIssuanceRepository
{
    TokenIssuance? GetTokenIssuance(int id);
    int AddTokenIssuance(TokenIssuance tokenIssuance);
}