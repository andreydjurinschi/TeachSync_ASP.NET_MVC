using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachSyncApp.Migrations
{
    /// <inheritdoc />
    public partial class Фввштпыысрувгдуещтщешашсфешщт : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ScheduleId",
                table: "Notifications",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ScheduleId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Notifications");
        }
    }
}
