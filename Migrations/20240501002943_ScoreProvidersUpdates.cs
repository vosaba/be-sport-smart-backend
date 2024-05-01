using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ScoreProvidersUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Inputs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17a249df-3bde-4bc8-b09c-7f1d51a33b8c", null, "Trainer", "TRAINER" },
                    { "32c508a3-2b2c-40eb-bb93-53bf29edfd7b", null, "User", "USER" },
                    { "f34073a8-406f-4fe0-95ef-8eec51c64404", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17a249df-3bde-4bc8-b09c-7f1d51a33b8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32c508a3-2b2c-40eb-bb93-53bf29edfd7b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f34073a8-406f-4fe0-95ef-8eec51c64404");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Inputs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27da775d-1226-4888-8e40-682c13925a68", null, "User", "USER" },
                    { "8dd59f95-36c9-4bd2-9ba5-77e64a9df353", null, "Admin", "ADMIN" },
                    { "9fc7e6a1-b2f2-4812-b1cb-d3d5db039409", null, "Trainer", "TRAINER" }
                });
        }
    }
}
