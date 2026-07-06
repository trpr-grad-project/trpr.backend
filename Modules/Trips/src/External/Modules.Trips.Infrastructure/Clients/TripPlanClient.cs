using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Modules.Trips.Infrastructure.Dtos.Requests;
using Modules.Trips.Infrastructure.Dtos.Responses;
using Modules.Trips.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Modules.Trips.Infrastructure.Clients;

public class TripPlanClient
{
    public HttpClient HttpClient { get; set; }
    public TripPlanOptions TripPlanOptions { get; set; }
    public ILogger<TripPlanClient> Logger { get; set; }
    public TripPlanClient(IOptions<TripPlanOptions> options, HttpClient httpClient, ILogger<TripPlanClient> logger)
    {
        TripPlanOptions = options.Value;
        HttpClient = httpClient;
        Logger = logger;
    }

    public async Task<TripPlanResponseDto> GeneratePlanAsync(TripPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        string baseUrl = TripPlanOptions.BaseUrl.TrimEnd('/');

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("Trip plan base URL is not configured.");

        var url = $"{baseUrl}/plan";
        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync(url, request, cancellationToken);
        Logger.LogInformation("Trip plan request sent to {Url} with payload: {@Request}", url, request);
        string responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        string response2Body = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            Logger.LogError("Failed to generate trip plan. Status Code: {StatusCode}, Response: {ResponseContent}", httpResponseMessage.StatusCode, responseContent);
            throw new InvalidOperationException($"Failed to generate trip plan. Status Code: {httpResponseMessage.StatusCode}, Response: {responseContent}");
        }
        httpResponseMessage.EnsureSuccessStatusCode();

        Logger.LogInformation("Trip plan generated successfully.");
        return await httpResponseMessage.Content.ReadFromJsonAsync<TripPlanResponseDto>(cancellationToken) ?? throw new InvalidOperationException("Failed to deserialize the trip plan response content.");
    }
}
