using Core.Base;
using Core.Utils.DB;

namespace Agency.AgencyModule.Repos;

public class AgencyRepository(INawaDaoRepository nawaDaoRepository, IQueryBuilderRepository queryBuilderRepository) : DalBase<Models.Agency>(nawaDaoRepository, queryBuilderRepository), IAgencyRepository
{
    public Models.Agency? GetAgency(int id)
    {
        return NawaDao.Get<Models.Agency>(id);
    }
}