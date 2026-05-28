namespace Modules.Trips.Application.Dtos.Responses
{
    public class BiddingCursorPageDto
    {
        public ICollection<BiddingResponseDto> Items { get; set; } = [];
        public string? NextCursor { get; set; }
        public bool HasNextPage => NextCursor != null;
    }
}
