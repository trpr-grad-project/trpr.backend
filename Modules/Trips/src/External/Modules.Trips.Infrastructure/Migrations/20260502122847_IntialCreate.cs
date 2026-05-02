using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "trp");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "governorates",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_governorates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumer_messages", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_consumer_messages",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_consumer_messages", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "places",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    average_visit_time = table.Column<double>(type: "double precision", nullable: true),
                    osrm_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    visit_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    rate_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    governorate_id = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<Point>(type: "geography (Point, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_places", x => x.id);
                    table.ForeignKey(
                        name: "fk_places_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "trp",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_places_governorates_governorate_id",
                        column: x => x.governorate_id,
                        principalSchema: "trp",
                        principalTable: "governorates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "theme_categories",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    max_limit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_theme_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_theme_categories_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "trp",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_theme_categories_themes_theme_id",
                        column: x => x.theme_id,
                        principalSchema: "trp",
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "theme_tags",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_theme_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_theme_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "trp",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_theme_tags_themes_theme_id",
                        column: x => x.theme_id,
                        principalSchema: "trp",
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place_tags",
                schema: "trp",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    place_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_tags", x => new { x.place_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_place_tags_places_place_id",
                        column: x => x.place_id,
                        principalSchema: "trp",
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "trp",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_place_tags_tag_id",
                schema: "trp",
                table: "place_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_category_id",
                schema: "trp",
                table: "places",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_governorate_id",
                schema: "trp",
                table: "places",
                column: "governorate_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_location",
                schema: "trp",
                table: "places",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_theme_categories_category_id",
                schema: "trp",
                table: "theme_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_categories_theme_id",
                schema: "trp",
                table: "theme_categories",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_tags_tag_id",
                schema: "trp",
                table: "theme_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_tags_theme_id",
                schema: "trp",
                table: "theme_tags",
                column: "theme_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "place_tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "theme_categories",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "theme_tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "places",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "themes",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "governorates",
                schema: "trp");
        }
    }
}
