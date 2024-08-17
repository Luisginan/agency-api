// Purpose: Interface for the Agency service.
namespace Agency.AgencyModule.Services;

public interface IAgencyService
{
    Models.Agency? GetAgency(int id);
}