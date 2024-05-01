using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ScoreProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f463188-bc42-4914-aa04-c44657ad389a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f122808e-77e6-4718-b733-08837a0d6940");

            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DependentProviders = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreProviderInputs",
                columns: table => new
                {
                    ScoreProviderId = table.Column<int>(type: "integer", nullable: false),
                    InputId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreProviderInputs", x => new { x.ScoreProviderId, x.InputId });
                    table.ForeignKey(
                        name: "FK_ScoreProviderInputs_Inputs_InputId",
                        column: x => x.InputId,
                        principalTable: "Inputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreProviderInputs_ScoreProviders_ScoreProviderId",
                        column: x => x.ScoreProviderId,
                        principalTable: "ScoreProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27da775d-1226-4888-8e40-682c13925a68", null, "User", "USER" },
                    { "8dd59f95-36c9-4bd2-9ba5-77e64a9df353", null, "Admin", "ADMIN" },
                    { "9fc7e6a1-b2f2-4812-b1cb-d3d5db039409", null, "Trainer", "TRAINER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreProviderInputs_InputId",
                table: "ScoreProviderInputs",
                column: "InputId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreProviderInputs");

            migrationBuilder.DropTable(
                name: "Inputs");

            migrationBuilder.DropTable(
                name: "ScoreProviders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27da775d-1226-4888-8e40-682c13925a68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8dd59f95-36c9-4bd2-9ba5-77e64a9df353");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fc7e6a1-b2f2-4812-b1cb-d3d5db039409");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f497b30-8719-4836-af5f-3f9e984412a3", null, "User", "USER" },
                    { "5b409d0d-cc11-47e7-b395-1bb8c707a5de", null, "Admin", "ADMIN" }
                });
        }
    }
}
