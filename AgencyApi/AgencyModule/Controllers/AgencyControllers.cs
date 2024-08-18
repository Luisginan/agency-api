using AgencyApi.AgencyModule.Dto;
using AgencyApi.AgencyModule.Services;
using AutoMapper;
using Core.Base;
using Core.Utils.DB;
using Microsoft.AspNetCore.Mvc;

namespace AgencyApi.AgencyModule.Controllers;
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AgencyControllers(
    ICache cache, 
    ILogger<AgencyControllers> logger, 
    IAgencyService agencyService, 
    IMapper mapper,
    IConnection dbConnection) : SuperController(cache, logger, dbConnection)
{
    protected override string CacheKeyRoot => "AgencyApi.AgencyModule.Controllers.AgencyController";
    
    [HttpGet]
    [ProducesResponseType(typeof(AgencyResponseDto), 200)]
    
    public async Task<IActionResult> GetAgency(int id)
    {
        var agency = await UseCacheAsync($"{CacheKeyRoot}.GetAgency.{id}", () =>
        {
            var agency = agencyService.GetAgency(id);
            return Task.FromResult(agency);
        });
        if (agency == null)
            return NotFound();
        
        var response = mapper.Map<AgencyResponseDto>(agency);
        return Ok(response);
    }
}