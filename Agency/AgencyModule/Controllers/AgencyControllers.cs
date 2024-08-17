using Agency.AgencyModule.Services;
using Core.Base;
using Core.Utils.DB;
using Microsoft.AspNetCore.Mvc;

namespace Agency.AgencyModule.Controllers;

public class AgencyControllers(ICache cache, ILogger logger, IAgencyService agencyService, IConnection dbConnection) : SuperController(cache, logger, dbConnection)
{
    protected override string CacheKeyRoot => "Agency.AgencyModule.Controllers.AgencyController";
    
    [HttpGet]
    public async Task<IActionResult> GetAgency(int id)
    {
        var agency = await UseCacheAsync($"{CacheKeyRoot}.GetAgency.{id}", () =>
        {
            var agency = agencyService.GetAgency(id);
            return Task.FromResult(agency);
        });
        if (agency == null)
            return NotFound();
        
        return Ok(agency);
    }
}