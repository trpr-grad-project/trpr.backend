using Microsoft.AspNetCore.SignalR;

namespace Modules.Trips.Application.Dtos.Responses;

public class DayResponseDto
{
    public int Day { get; set; }
    public double Duration { get; set; }
    public DateTime DayDate { get; set; }
    public List<PlaceDto> Places { get; set; } = [];
}
