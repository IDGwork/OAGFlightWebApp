namespace OAGFlightWebApp.Models;

public class Journey
{
    public int RecommendationId { get; set; }
    public string Direction { get; set; }
    public List<Flight> Flights { get; set; }
}