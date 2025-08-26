using OAGFlightWebApp.DTOs;
using OAGFlightWebApp.Models;
using OAGFlightWebApp.Repositories;

namespace OAGFlightWebApp.Services;

public class FlightAnalysisService : IFlightAnalysisService
{
    private readonly IFlightRepository _flightRepo;

    public FlightAnalysisService(IFlightRepository flightRepo)
    {
        _flightRepo = flightRepo;
    }

    public async Task<List<RoundtripDto>> GetAllRoundtripCombinationsAsync()
    {
        var journeys = await _flightRepo.GetJourneysAsync();
        var availabilities = (await _flightRepo.GetAvailabilitiesAsync())
            .ToDictionary(a => a.RecommendationId, a => a.Total);

        var inbound = journeys.Where(j => j.Direction == "I").ToList();
        var outbound = journeys.Where(j => j.Direction == "V").ToList();

        var result = new List<RoundtripDto>();

        foreach (var inJ in inbound)
        {
            foreach (var outJ in outbound)
            {
                if (inJ.RecommendationId == outJ.RecommendationId &&
                    inJ.Flights.Last().ArrivalDate < outJ.Flights.First().DepartureDate)
                {
                    var total = availabilities.TryGetValue(inJ.RecommendationId, out var price) ? price : 0;
                    result.Add(ToDto(inJ, outJ, total));
                }
            }
        }

        return result;
    }

    public async Task<List<PriceResultDto>> GetCheapestPerRoundtripAsync()
    {
        var roundtrips = await GetAllRoundtripCombinationsAsync();

        return roundtrips
            .GroupBy(r => $"{r.Outbound.Flights.First().FromCode}_{r.Outbound.Flights.Last().ToCode}__{r.Return.Flights.First().FromCode}_{r.Return.Flights.Last().ToCode}")
            .Select(g => ToDto(g.Key, g.Min(r => r.TotalPrice)))
            .ToList();
    }

    public async Task<List<RoundtripDto>> GetDuplicateInboundDifferentPriceAsync()
    {
        var roundtrips = await GetAllRoundtripCombinationsAsync();

        return roundtrips
            .GroupBy(r => $"{r.Outbound.Flights.First().Number}_{r.Return.Flights.Select(f => f.Number).FirstOrDefault()}")
            .Where(g => g.Select(x => x.TotalPrice).Distinct().Count() > 1)
            .SelectMany(g => g)
            .ToList();
    }

    public async Task<List<PriceResultDto>> GetRoundtripsGroupedByPriceAsync()
    {
        var roundtrips = await GetAllRoundtripCombinationsAsync();

        return roundtrips
            .GroupBy(r => $"{r.Outbound.Flights.First().FromCode}_{r.Outbound.Flights.Last().ToCode}__{r.Return.Flights.First().FromCode}_{r.Return.Flights.Last().ToCode}")
            .Select(g => ToDto(g.Key, g.Sum(r => r.TotalPrice)))
            .ToList();
    }

    // -------------------- Mapping Helpers --------------------

    private static FlightDto ToDto(Flight flight) => new()
    {
        Number = flight.Number,
        Airline = string.IsNullOrWhiteSpace(flight.Operator) || flight.Operator == "0"
            ? flight.CompanyCode
            : flight.Operator,
        From = flight.DepartureAirportName,
        FromCode = flight.DepartureAirportCode,
        To = flight.ArrivalAirportName,
        ToCode = flight.ArrivalAirportCode,
        Departure = flight.DepartureDate,
        Arrival = flight.ArrivalDate,
        CabinClass = flight.CabinClass
    };

    private static JourneyDto ToDto(Journey journey) => new()
    {
        RecommendationId = journey.RecommendationId,
        Direction = journey.Direction,
        Flights = journey.Flights.Select(ToDto).ToList()
    };

    private static RoundtripDto ToDto(Journey outbound, Journey inbound, decimal totalPrice) => new()
    {
        Outbound = ToDto(outbound),
        Return = ToDto(inbound),
        TotalPrice = totalPrice
    };

    private static PriceResultDto ToDto(string roundtripKey, decimal price) => new()
    {
        RoundtripKey = roundtripKey,
        Price = price
    };
}