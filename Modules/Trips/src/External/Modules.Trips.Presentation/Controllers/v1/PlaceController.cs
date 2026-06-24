using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Presentation.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/places")]
public class PlaceController(PlaceService placeService) : ControllerBase
{
    public Guid UserId => User.GetUserId();

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PlaceDto>> CreateAdminPlace([FromBody] CreatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.CreatePlaceAsync(null, request, cancellationToken);
        return Ok(place);
    }


    [HttpPost("me")]
    public async Task<ActionResult<PlaceDto>> CreatePlace([FromBody] CreatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.CreatePlaceAsync(UserId, request, cancellationToken);
        return Ok(place);
    }

    [HttpGet]
    public async Task<ActionResult<CursorPageDto<PlaceDto, int?>>> GetPlaces([FromQuery] GetPlacesQueryDto query, CancellationToken cancellationToken = default)
    {
        var places = await placeService.GetPlacesAsync(null, query, cancellationToken);
        return Ok(places);
    }

    [HttpGet("me")]
    public async Task<ActionResult<CursorPageDto<PlaceDto, int?>>> GetPersonalPlaces([FromQuery] GetPlacesQueryDto query, CancellationToken cancellationToken = default)
    {
        var places = await placeService.GetPlacesAsync(UserId, query, cancellationToken);
        return Ok(places);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PlaceDto>> UpdateAdminPlace(int id, [FromBody] UpdatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.UpdatePlaceAsync(null, id, request, cancellationToken);
        return Ok(place);
    }


    [HttpPut("me/{id}")]
    public async Task<ActionResult<PlaceDto>> UpdatePlace(int id, [FromBody] UpdatePlaceRequestDto request, CancellationToken cancellationToken = default)
    {
        var place = await placeService.UpdatePlaceAsync(UserId, id, request, cancellationToken);
        return Ok(place);
    }

    [HttpGet("form-data")]
    public async Task<ActionResult<PlaceFormDataDto>> GetPlaceFormData(CancellationToken cancellationToken = default)
    {
        var formData = await placeService.GetPlaceFormDataAsync(cancellationToken);
        return Ok(formData);
    }
}
