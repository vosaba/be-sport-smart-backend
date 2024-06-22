using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bss.Dal.Migrations.Migrations.Core
{
    /// <inheritdoc />
    public partial class AddEngineColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Engine",
                table: "Computations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Computations_Engine",
                table: "Computations",
                column: "Engine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Computations_Engine",
                table: "Computations");

            migrationBuilder.DropColumn(
                name: "Engine",
                table: "Computations");
        }
    }
}
