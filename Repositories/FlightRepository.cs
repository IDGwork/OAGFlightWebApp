using System.Text.Json;
using OAGFlightWebApp.Models;

namespace OAGFlightWebApp.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly HttpClient _httpClient;
    private const string Url = "http://homeworktask.infare.lt/search.php?from=JFK&to=AUH&depart=2027-01-11&return=2027-01-18";

    public FlightRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Journey>> GetJourneysAsync()
    {
        var root = await FetchDataAsync();
        return MapJourneys(root.Body.Data.Journeys);
    }

    public async Task<List<Availability>> GetAvailabilitiesAsync()
    {
        var root = await FetchDataAsync();
        return root.Body.Data.TotalAvailabilities
            .Select(a => new Availability
            {
                RecommendationId = a.RecommendationId,
                Total = a.Total
            })
            .ToList();
    }

    private async Task<Root> FetchDataAsync()
    {
        var json = await _httpClient.GetStringAsync(Url);
        return JsonSerializer.Deserialize<Root>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private List<Journey> MapJourneys(List<JourneyRaw> rawJourneys)
    {
        var result = new List<Journey>();

        foreach (var j in rawJourneys)
        {
            var flights = j.Flights.Select(f => new Flight
            {
                Number = f.Number,
                CompanyCode = f.CompanyCode,
                Operator = f.Operator,
                DepartureAirportCode = f.AirportDeparture.Code,
                ArrivalAirportCode = f.AirportArrival.Code,
                DepartureAirportName = f.AirportDeparture.Name,
                ArrivalAirportName = f.AirportArrival.Name,
                DepartureDate = DateTime.Parse(f.DateDeparture),
                ArrivalDate = DateTime.Parse(f.DateArrival),
                CabinClass = f.CabinClass.Description
            }).ToList();

            result.Add(new Journey
            {
                RecommendationId = j.RecommendationId,
                Direction = j.Direction,
                Flights = flights
            });
        }

        return result;
    }
    
    private class Root
    {
        public RootBody Body { get; set; }
    }

    private class RootBody
    {
        public RootData Data { get; set; }
    }

    private class RootData
    {
        public List<JourneyRaw> Journeys { get; set; }
        public List<AvailabilityRaw> TotalAvailabilities { get; set; }
    }

    private class AvailabilityRaw
    {
        public int RecommendationId { get; set; }
        public decimal Total { get; set; }
    }

    private class JourneyRaw
    {
        public int RecommendationId { get; set; }
        public string Direction { get; set; }
        public List<FlightRaw> Flights { get; set; }
    }

    private class FlightRaw
    {
        public string Number { get; set; }
        public string CompanyCode { get; set; }
        public string Operator { get; set; }
        public Airport AirportDeparture { get; set; }
        public Airport AirportArrival { get; set; }
        public string DateDeparture { get; set; }
        public string DateArrival { get; set; }
        public Cabin CabinClass { get; set; }
    }

    private class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    private class Cabin
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}