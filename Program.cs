using OAGFlightWebApp.Repositories;
using OAGFlightWebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IFlightRepository, FlightRepository>();

builder.Services.AddScoped<IFlightAnalysisService, FlightAnalysisService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();