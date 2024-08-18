using AgencyApi.TokenIssuanceModule.Models;

namespace AgencyApi.TokenIssuanceModule.Services;

public interface ITokenIssuanceService
{
    TokenIssuance GetTokenIssuance(int id);
    void AddTokenIssuance(TokenIssuance tokenIssuance);

}