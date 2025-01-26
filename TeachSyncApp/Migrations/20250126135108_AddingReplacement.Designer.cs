﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeachSyncApp.Context;

#nullable disable

namespace TeachSyncApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250126135108_AddingReplacement")]
    partial class AddingReplacement
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TeachSyncApp.Models.ClassRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ClassRooms", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.Courses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.Replacement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovedById")
                        .HasColumnType("int");

                    b.Property<int>("CourseTopicId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestRime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedById");

                    b.HasIndex("CourseTopicId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Replacements");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassRoomId")
                        .HasColumnType("int");

                    b.Property<int>("DayOfWeekId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("GroupCourseId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassRoomId");

                    b.HasIndex("DayOfWeekId");

                    b.HasIndex("GroupCourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Topics", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TeachSyncApp.Models.WeekDays", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DaysOfWeek");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.CourseTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("TopicId");

                    b.ToTable("CoursesTopics");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.GroupCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupCourses");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Courses", b =>
                {
                    b.HasOne("TeachSyncApp.Models.User", "User")
                        .WithMany("Courses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Replacement", b =>
                {
                    b.HasOne("TeachSyncApp.Models.User", "TeacherApprove")
                        .WithMany("Replacements")
                        .HasForeignKey("ApprovedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.intermediateModels.CourseTopic", "CourseTopic")
                        .WithMany("Replacements")
                        .HasForeignKey("CourseTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.Schedule", "Schedule")
                        .WithMany("Replacements")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CourseTopic");

                    b.Navigation("Schedule");

                    b.Navigation("TeacherApprove");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Schedule", b =>
                {
                    b.HasOne("TeachSyncApp.Models.ClassRoom", "ClassRoom")
                        .WithMany("Schedules")
                        .HasForeignKey("ClassRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.WeekDays", "WeekDays")
                        .WithMany("Schedules")
                        .HasForeignKey("DayOfWeekId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.intermediateModels.GroupCourse", "GroupCourse")
                        .WithMany("Schedules")
                        .HasForeignKey("GroupCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.User", "Teacher")
                        .WithMany("Schedules")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassRoom");

                    b.Navigation("GroupCourse");

                    b.Navigation("Teacher");

                    b.Navigation("WeekDays");
                });

            modelBuilder.Entity("TeachSyncApp.Models.User", b =>
                {
                    b.HasOne("TeachSyncApp.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.CourseTopic", b =>
                {
                    b.HasOne("TeachSyncApp.Models.Courses", "Course")
                        .WithMany("CoursesTopics")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.Topic", "Topic")
                        .WithMany("CoursesTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.GroupCourse", b =>
                {
                    b.HasOne("TeachSyncApp.Models.Courses", "Course")
                        .WithMany("GroupCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeachSyncApp.Models.Group", "Group")
                        .WithMany("GroupCourses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("TeachSyncApp.Models.ClassRoom", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Courses", b =>
                {
                    b.Navigation("CoursesTopics");

                    b.Navigation("GroupCourses");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Group", b =>
                {
                    b.Navigation("GroupCourses");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Schedule", b =>
                {
                    b.Navigation("Replacements");
                });

            modelBuilder.Entity("TeachSyncApp.Models.Topic", b =>
                {
                    b.Navigation("CoursesTopics");
                });

            modelBuilder.Entity("TeachSyncApp.Models.User", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Replacements");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("TeachSyncApp.Models.WeekDays", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.CourseTopic", b =>
                {
                    b.Navigation("Replacements");
                });

            modelBuilder.Entity("TeachSyncApp.Models.intermediateModels.GroupCourse", b =>
                {
                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
