public class FlightDto
{
    public string Number { get; set; }
    public string Airline { get; set; } 
    public string From { get; set; }
    public string To { get; set; }
    public string FromCode { get; set; }
    public string ToCode { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Arrival { get; set; }
    public string CabinClass { get; set; }
}