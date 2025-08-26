namespace OAGFlightWebApp.DTOs;

public class JourneyDto
{
    public int RecommendationId { get; set; }
    public string Direction { get; set; }
    public List<FlightDto> Flights { get; set; }
}