using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachSyncApp.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplacementResponses_Users_TeacherId",
                table: "ReplacementResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplacementResponses_Users_TeacherId",
                table: "ReplacementResponses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplacementResponses_Users_TeacherId",
                table: "ReplacementResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplacementResponses_Users_TeacherId",
                table: "ReplacementResponses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
