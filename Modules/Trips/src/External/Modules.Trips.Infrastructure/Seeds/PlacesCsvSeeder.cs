using Common.Application.Seeds;

namespace Modules.Trips.Infrastructure.Seeds;

public class PlacesCsvSeeder : ISeed
{
    public Task SeedAsync()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seeds", "places.csv");
        // Add logic to read and process the CSV file
        return Task.CompletedTask;
    }

}
