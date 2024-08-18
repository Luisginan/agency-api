using AgencyApi.TokenIssuanceModule.Models;
using AgencyApi.TokenIssuanceModule.Repos;
using Core.CExceptions;

namespace AgencyApi.TokenIssuanceModule.Services;

public class TokenIssuanceService(ITokenIssuanceRepository tokenIssuanceRepository) : ITokenIssuanceService
{
    public TokenIssuance GetTokenIssuance(int id)
    {
        var token = tokenIssuanceRepository.GetTokenIssuance(id);
        if (token == null)
            throw new ServiceException("Token not found");

        return token;
    }

    public void AddTokenIssuance(TokenIssuance tokenIssuance)
    {
        tokenIssuanceRepository.AddTokenIssuance(tokenIssuance);
    }
}