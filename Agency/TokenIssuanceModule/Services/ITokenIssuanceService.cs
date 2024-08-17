using Agency.TokenIssuanceModule.Models;

namespace Agency.TokenIssuanceModule.Services;

public interface ITokenIssuanceService
{
    TokenIssuance GetTokenIssuance(int id);
    void AddTokenIssuance(TokenIssuance tokenIssuance);

}