using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _APP2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "difficulty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_difficulty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", maxLength: 3, nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RegionImageUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "walk",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    LengthInKm = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    WalkImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    DifficultyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RegionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_walk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_walk_difficulty_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "difficulty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_walk_region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_walk_DifficultyId",
                table: "walk",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_walk_RegionId",
                table: "walk",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "walk");

            migrationBuilder.DropTable(
                name: "difficulty");

            migrationBuilder.DropTable(
                name: "region");
        }
    }
}
