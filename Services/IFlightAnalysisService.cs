using OAGFlightWebApp.DTOs;

namespace OAGFlightWebApp.Services;

public interface IFlightAnalysisService
{
    Task<List<RoundtripDto>> GetAllRoundtripCombinationsAsync();
    Task<List<PriceResultDto>> GetCheapestPerRoundtripAsync();
    Task<List<RoundtripDto>> GetDuplicateInboundDifferentPriceAsync();
    Task<List<PriceResultDto>> GetRoundtripsGroupedByPriceAsync();
}