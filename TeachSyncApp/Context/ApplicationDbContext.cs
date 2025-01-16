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
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<Topic>().ToTable("Topics");
        modelBuilder.Entity<Courses>().ToTable("Courses");
        modelBuilder.Entity<ClassRoom>().ToTable("ClassRooms");
        modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Courses>().HasOne(c => c.User).WithMany(u => u.Courses).HasForeignKey(c => c.TeacherId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<CourseTopic>().HasKey(ct => ct.Id);
        modelBuilder.Entity<CourseTopic>().HasOne(ct => ct.Course).WithMany(c => c.CoursesTopics).HasForeignKey(ct => ct.CourseId);
        modelBuilder.Entity<CourseTopic>().HasOne(ct => ct.Topic).WithMany(t => t.CoursesTopics).HasForeignKey(ct => ct.TopicId);
        modelBuilder.Entity<GroupCourse>().HasKey(cc => cc.Id);
        modelBuilder.Entity<GroupCourse>().HasOne(cc => cc.Group).WithMany(c => c.GroupCourses).HasForeignKey(cc => cc.GroupId);
        modelBuilder.Entity<GroupCourse>().HasOne(cc => cc.Course).WithMany(c => c.GroupCourses).HasForeignKey(cc => cc.CourseId);
    }

}