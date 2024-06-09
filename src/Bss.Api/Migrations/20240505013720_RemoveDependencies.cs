using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bss.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreProviderInputs");

            migrationBuilder.AddColumn<string[]>(
                name: "DependentInputs",
                table: "ScoreProviders",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependentInputs",
                table: "ScoreProviders");

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


            migrationBuilder.CreateIndex(
                name: "IX_ScoreProviderInputs_InputId",
                table: "ScoreProviderInputs",
                column: "InputId");
        }
    }
}
