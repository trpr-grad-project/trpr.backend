namespace Modules.Trips.Application.Dtos.Requests
{
    public class GetTripBiddingsQueryDto
    {
        public string? Cursor { get; set; }
        public int PageSize { get; set; } = 10;
        public BiddingSortOrder SortOrder { get; set; } = BiddingSortOrder.Oldest;
    }
}
