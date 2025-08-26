using OAGFlightWebApp.Models;

namespace OAGFlightWebApp.Repositories;

public interface IFlightRepository
{
    Task<List<Journey>> GetJourneysAsync();
    Task<List<Availability>> GetAvailabilitiesAsync();
}