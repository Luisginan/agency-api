// Purpose: Interface for the AgencyApi service.
namespace AgencyApi.AgencyModule.Services;

public interface IAgencyService
{
    Models.Agency? GetAgency(int id);
}