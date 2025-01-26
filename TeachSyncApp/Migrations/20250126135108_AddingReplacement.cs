using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachSyncApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingReplacement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Replacements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    CourseTopicId = table.Column<int>(type: "int", nullable: false),
                    RequestRime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedById = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replacements_CoursesTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CoursesTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replacements_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Replacements_Users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_ApprovedById",
                table: "Replacements",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_CourseTopicId",
                table: "Replacements",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_ScheduleId",
                table: "Replacements",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Replacements");
        }
    }
}
