using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bss.Dal.Migrations.Migrations.Core
{
    /// <inheritdoc />
    public partial class AddOrderAndAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InputSource",
                table: "Measures",
                newName: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Availability",
                table: "Measures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "Measures",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinValue",
                table: "Measures",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Availability",
                table: "Computations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Computations");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Measures",
                newName: "InputSource");
        }
    }
}
