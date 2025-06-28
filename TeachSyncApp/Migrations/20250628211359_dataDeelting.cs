using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachSyncApp.Migrations
{
    /// <inheritdoc />
    public partial class dataDeelting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesTopics_Courses_CourseId",
                table: "CoursesTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesTopics_Topics_TopicId",
                table: "CoursesTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCourses_Courses_CourseId",
                table: "GroupCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCourses_Groups_GroupId",
                table: "GroupCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Replacements_ReplacementId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_TeacherId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_CoursesTopics_CourseTopicId",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_Schedules_ScheduleId",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_Users_ApprovedById",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ClassRooms_ClassRoomId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_DaysOfWeek_DayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_GroupCourses_GroupCourseId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_TeacherId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ReplacementResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementResponses_UserId",
                table: "ReplacementResponses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesTopics_Courses_CourseId",
                table: "CoursesTopics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesTopics_Topics_TopicId",
                table: "CoursesTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCourses_Courses_CourseId",
                table: "GroupCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCourses_Groups_GroupId",
                table: "GroupCourses",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Replacements_ReplacementId",
                table: "Notifications",
                column: "ReplacementId",
                principalTable: "Replacements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_TeacherId",
                table: "Notifications",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReplacementResponses_Users_UserId",
                table: "ReplacementResponses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_CoursesTopics_CourseTopicId",
                table: "Replacements",
                column: "CourseTopicId",
                principalTable: "CoursesTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_Schedules_ScheduleId",
                table: "Replacements",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_Users_ApprovedById",
                table: "Replacements",
                column: "ApprovedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ClassRooms_ClassRoomId",
                table: "Schedules",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_DaysOfWeek_DayOfWeekId",
                table: "Schedules",
                column: "DayOfWeekId",
                principalTable: "DaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_GroupCourses_GroupCourseId",
                table: "Schedules",
                column: "GroupCourseId",
                principalTable: "GroupCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesTopics_Courses_CourseId",
                table: "CoursesTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesTopics_Topics_TopicId",
                table: "CoursesTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCourses_Courses_CourseId",
                table: "GroupCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCourses_Groups_GroupId",
                table: "GroupCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Replacements_ReplacementId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_TeacherId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ReplacementResponses_Users_UserId",
                table: "ReplacementResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_CoursesTopics_CourseTopicId",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_Schedules_ScheduleId",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Replacements_Users_ApprovedById",
                table: "Replacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ClassRooms_ClassRoomId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_DaysOfWeek_DayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_GroupCourses_GroupCourseId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_TeacherId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ReplacementResponses_UserId",
                table: "ReplacementResponses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReplacementResponses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesTopics_Courses_CourseId",
                table: "CoursesTopics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesTopics_Topics_TopicId",
                table: "CoursesTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCourses_Courses_CourseId",
                table: "GroupCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCourses_Groups_GroupId",
                table: "GroupCourses",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Replacements_ReplacementId",
                table: "Notifications",
                column: "ReplacementId",
                principalTable: "Replacements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Schedules_ScheduleId",
                table: "Notifications",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_TeacherId",
                table: "Notifications",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_CoursesTopics_CourseTopicId",
                table: "Replacements",
                column: "CourseTopicId",
                principalTable: "CoursesTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_Schedules_ScheduleId",
                table: "Replacements",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replacements_Users_ApprovedById",
                table: "Replacements",
                column: "ApprovedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ClassRooms_ClassRoomId",
                table: "Schedules",
                column: "ClassRoomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_DaysOfWeek_DayOfWeekId",
                table: "Schedules",
                column: "DayOfWeekId",
                principalTable: "DaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_GroupCourses_GroupCourseId",
                table: "Schedules",
                column: "GroupCourseId",
                principalTable: "GroupCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
