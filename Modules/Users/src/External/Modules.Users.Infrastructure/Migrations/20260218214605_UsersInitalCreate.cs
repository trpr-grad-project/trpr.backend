using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsersInitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "usr");

            migrationBuilder.CreateTable(
                name: "inbox_consumer_message",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumer_message", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "usr",
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
                name: "interests",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    native_name = table.Column<string>(type: "text", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_consumer_messages",
                schema: "usr",
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
                schema: "usr",
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
                name: "users",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    identity_provider_id = table.Column<string>(type: "text", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vibes",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    thumbnail = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_profiles_users_id",
                        column: x => x.id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_interests",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    interest_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_interests", x => new { x.profile_id, x.interest_id });
                    table.ForeignKey(
                        name: "fk_profile_interests_interests_interest_id",
                        column: x => x.interest_id,
                        principalSchema: "usr",
                        principalTable: "interests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_interests_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_languages",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_languages", x => new { x.profile_id, x.language_id });
                    table.ForeignKey(
                        name: "fk_profile_languages_languages_language_id",
                        column: x => x.language_id,
                        principalSchema: "usr",
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_languages_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_vibes",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vibe_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_vibes", x => new { x.profile_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_profile_vibes_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_vibes_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalSchema: "usr",
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_profile_interests_interest_id",
                schema: "usr",
                table: "profile_interests",
                column: "interest_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_languages_language_id",
                schema: "usr",
                table: "profile_languages",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_vibes_vibe_id",
                schema: "usr",
                table: "profile_vibes",
                column: "vibe_id");

            migrationBuilder.CreateIndex(
                name: "ix_tokens_user_id",
                schema: "usr",
                table: "tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_identity_provider_id",
                schema: "usr",
                table: "users",
                column: "identity_provider_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                schema: "usr",
                table: "users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumer_message",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_interests",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_languages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_vibes",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "tokens",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "interests",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "languages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profiles",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "vibes",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "users",
                schema: "usr");
        }
    }
}
