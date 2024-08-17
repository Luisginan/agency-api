using Agency.TokenIssuanceModule.Models;

namespace Agency.TokenIssuanceModule.Repos;

public interface ITokenIssuanceRepository
{
    TokenIssuance? GetTokenIssuance(int id);
    long AddTokenIssuance(TokenIssuance tokenIssuance);
}