using Agency.AgencyModule.Repos;

namespace Agency.AgencyModule.Services;

public class AgencyService(IAgencyRepository agencyRepository) : IAgencyService
{
    public Models.Agency? GetAgency(int id)
    {
        return agencyRepository.GetAgency(id);
    }
}