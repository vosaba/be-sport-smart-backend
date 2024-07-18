using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bss.Dal.Migrations.Migrations.Core
{
    /// <inheritdoc />
    public partial class AddUserMeasureValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMeasureValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeasureValues", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasureValues_Name",
                table: "UserMeasureValues",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasureValues_UserId",
                table: "UserMeasureValues",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMeasureValues");
        }
    }
}
