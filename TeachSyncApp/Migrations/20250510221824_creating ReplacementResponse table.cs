using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachSyncApp.Migrations
{
    /// <inheritdoc />
    public partial class creatingReplacementResponsetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplacementResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplacementId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResponsedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplacementResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplacementResponses_Replacements_ReplacementId",
                        column: x => x.ReplacementId,
                        principalTable: "Replacements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplacementResponses_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementResponses_ReplacementId",
                table: "ReplacementResponses",
                column: "ReplacementId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementResponses_TeacherId",
                table: "ReplacementResponses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplacementResponses");
        }
    }
}
