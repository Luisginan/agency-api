using Agency.TokenIssuanceModule.Models;
using Core.Base;
using Core.Utils.DB;

namespace Agency.TokenIssuanceModule.Repos;

public class TokenIssuanceRepository(
    INawaDaoRepository nawaDaoRepository,
    IQueryBuilderRepository queryBuilderRepository)
    : DalBase<TokenIssuance>(nawaDaoRepository, queryBuilderRepository), ITokenIssuanceRepository
{
    public TokenIssuance? GetTokenIssuance(int id)
    {
        return Get(id);
    }

    public long AddTokenIssuance(TokenIssuance tokenIssuance)
    {
      return Insert2(tokenIssuance);
    }
}