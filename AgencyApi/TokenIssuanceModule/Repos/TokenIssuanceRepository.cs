using AgencyApi.TokenIssuanceModule.Models;
using Core.Base;
using Core.Utils.DB;

namespace AgencyApi.TokenIssuanceModule.Repos;

public class TokenIssuanceRepository(
    INawaDaoRepository nawaDaoRepository,
    IQueryBuilderRepository queryBuilderRepository)
    : DalBase<TokenIssuance>(nawaDaoRepository, queryBuilderRepository), ITokenIssuanceRepository
{
    public TokenIssuance? GetTokenIssuance(int id)
    {
        return Get(id);
    }

    public int AddTokenIssuance(TokenIssuance tokenIssuance)
    {
      return Insert2(tokenIssuance);
    }
}