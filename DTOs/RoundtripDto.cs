namespace OAGFlightWebApp.DTOs;

public class RoundtripDto
{
    public JourneyDto Outbound { get; set; }
    public JourneyDto Return { get; set; }
    public decimal TotalPrice { get; set; }
}