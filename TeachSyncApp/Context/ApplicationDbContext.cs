using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Models;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Courses> Courses { get; set; }
    public DbSet<CourseTopic> CoursesTopics { get; set; }
    public DbSet<ClassRoom> ClassRooms { get; set; }
    public DbSet<GroupCourse> GroupCourses { get; set; }
    public DbSet<WeekDays> DaysOfWeek { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Replacement> Replacements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<Topic>().ToTable("Topics");
        modelBuilder.Entity<Courses>().ToTable("Courses");
        modelBuilder.Entity<ClassRoom>().ToTable("ClassRooms");

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Courses>()
            .HasOne(c => c.User)
            .WithMany(u => u.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseTopic>().HasKey(ct => ct.Id);
        modelBuilder.Entity<CourseTopic>()
            .HasOne(ct => ct.Course)
            .WithMany(c => c.CoursesTopics)
            .HasForeignKey(ct => ct.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseTopic>()
            .HasOne(ct => ct.Topic)
            .WithMany(t => t.CoursesTopics)
            .HasForeignKey(ct => ct.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupCourse>().HasKey(gc => gc.Id);
        modelBuilder.Entity<GroupCourse>()
            .HasOne(gc => gc.Group)
            .WithMany(g => g.GroupCourses)
            .HasForeignKey(gc => gc.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupCourse>()
            .HasOne(gc => gc.Course)
            .WithMany(c => c.GroupCourses)
            .HasForeignKey(gc => gc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>().HasKey(s => s.Id);
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Teacher)
            .WithMany(t => t.Schedules)
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.ClassRoom)
            .WithMany(cr => cr.Schedules)
            .HasForeignKey(s => s.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.GroupCourse)
            .WithMany(gc => gc.Schedules)
            .HasForeignKey(s => s.GroupCourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.WeekDays)
            .WithMany(wd => wd.Schedules)
            .HasForeignKey(s => s.DayOfWeekId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Replacement>().HasKey(r => r.Id);
        modelBuilder.Entity<Replacement>()
            .Property(r => r.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Replacement>()
            .HasOne(r => r.TeacherApprove)
            .WithMany(t => t.Replacements)
            .HasForeignKey(r => r.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Replacement>()
            .HasOne(r => r.Schedule)
            .WithMany(s => s.Replacements)
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Replacement>()
            .HasOne(r => r.CourseTopic)
            .WithMany(ct => ct.Replacements)
            .HasForeignKey(r => r.CourseTopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
