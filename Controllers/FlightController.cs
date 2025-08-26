using Microsoft.AspNetCore.Mvc;
using OAGFlightWebApp.Services;

namespace OAGFlightWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightAnalysisService _service;

    public FlightsController(IFlightAnalysisService service)
    {
        _service = service;
    }

    [HttpGet("roundtrips")]
    public async Task<IActionResult> GetAllRoundtrips()
        => Ok(await _service.GetAllRoundtripCombinationsAsync());

    [HttpGet("cheapest")]
    public async Task<IActionResult> GetCheapestPerRoundtrip()
        => Ok(await _service.GetCheapestPerRoundtripAsync());

    [HttpGet("duplicates")]
    public async Task<IActionResult> GetDuplicateInboundDifferentPrice()
        => Ok(await _service.GetDuplicateInboundDifferentPriceAsync());

    [HttpGet("grouped-by-price")]
    public async Task<IActionResult> GetRoundtripsGroupedByPrice()
        => Ok(await _service.GetRoundtripsGroupedByPriceAsync());
}