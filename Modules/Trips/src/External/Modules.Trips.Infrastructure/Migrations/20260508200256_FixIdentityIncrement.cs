using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // fix the identity increment for the tags table
            migrationBuilder.Sql(@"
                SELECT setval(
                    pg_get_serial_sequence('trp.tags', 'id'),
                    (SELECT MAX(id) FROM trp.tags)
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
