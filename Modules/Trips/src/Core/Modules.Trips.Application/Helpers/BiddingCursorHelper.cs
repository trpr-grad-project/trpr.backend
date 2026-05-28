using System.Text;

namespace Modules.Trips.Application.Helpers
{
    internal static class BiddingCursorHelper
    {
        private const char Separator = '|';

        public static string EncodeTimeCursor(DateTime createdAt, Guid id)
        {
            var raw = $"{createdAt.Ticks}{Separator}{id}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public static string EncodePriceCursor(double price, DateTime createdAt, Guid id)
        {
            var raw = $"{price}{Separator}{createdAt.Ticks}{Separator}{id}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public static (DateTime CreatedAt, Guid Id) DecodeTimeCursor(string cursor)
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            var parts = raw.Split(Separator);
            if (parts.Length != 2)
                throw new FormatException("Invalid cursor format.");
            var createdAt = new DateTime(long.Parse(parts[0]), DateTimeKind.Utc);
            var id = Guid.Parse(parts[1]);
            return (createdAt, id);
        }

        public static (double Price, DateTime CreatedAt, Guid Id) DecodePriceCursor(string cursor)
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            var parts = raw.Split(Separator);
            if (parts.Length != 3)
                throw new FormatException("Invalid cursor format.");
            var price = double.Parse(parts[0]);
            var createdAt = new DateTime(long.Parse(parts[1]), DateTimeKind.Utc);
            var id = Guid.Parse(parts[2]);
            return (price, createdAt, id);
        }
    }
}
