using System.Globalization;
using Common.Application.Seeds;
using Microsoft.Extensions.Logging;
using Modules.Trips.Infrastructure.Seeds.Dto;
namespace Modules.Trips.Infrastructure.Seeds;

public class PlacesCsvSeeder(ILogger<PlacesCsvSeeder> logger) : ISeed
{
    public Task SeedAsync()
    {
        var seedsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seeds");

        var placeFiles = Directory.GetFiles(seedsDirectory, "places_*.csv");
        // Optional: sort them if order matters (e.g. version order)
        var orderedFiles = placeFiles
            .OrderBy(f =>
            {
                var fileName = Path.GetFileNameWithoutExtension(f); // places_1.0.1
                var versionPart = fileName.Replace("places_", "");
                return Version.Parse(versionPart);
            })
            .ToList();

        foreach (var file in orderedFiles)
        {
            logger.LogInformation("Processing seed file: {FileName}", file);
            ProcessCsvFile(file);
        }

        return Task.CompletedTask;
    }

    private void ProcessCsvFile(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<PlaceMap>();

        var records = csv.GetRecords<PlaceCsv>().ToList();

        foreach (var record in records)
        {
            logger.LogInformation("Read place: {PlaceName} at ({Lat}, {Lon}) with tags: {Tags}",
                record.PlaceName, record.Lat, record.Lon, record.Tags);
        }
    }

}
