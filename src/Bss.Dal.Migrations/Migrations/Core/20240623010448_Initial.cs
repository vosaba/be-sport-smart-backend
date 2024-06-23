using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bss.Dal.Migrations.Migrations.Core
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Formula = table.Column<string>(type: "text", nullable: false),
                    Engine = table.Column<int>(type: "integer", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequiredComputations = table.Column<List<string>>(type: "text[]", nullable: false),
                    RequiredMeasures = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    InputSource = table.Column<int>(type: "integer", nullable: false),
                    Options = table.Column<string[]>(type: "text[]", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Computations_Engine",
                table: "Computations",
                column: "Engine");

            migrationBuilder.CreateIndex(
                name: "IX_Computations_Name",
                table: "Computations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Computations_Type",
                table: "Computations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Measures_Name",
                table: "Measures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measures_Type",
                table: "Measures",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Computations");

            migrationBuilder.DropTable(
                name: "Measures");
        }
    }
}
