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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<Topic>().ToTable("Topics");
        modelBuilder.Entity<Courses>().ToTable("Courses");

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Courses>()
            .HasOne(c => c.User)         
            .WithMany(u => u.Courses)                      
            .HasForeignKey(c => c.TeacherId) 
            .OnDelete(DeleteBehavior.SetNull);
            
        modelBuilder.Entity<CourseTopic>().HasKey(ct => new { ct.CourseId, ct.TopicId });
        modelBuilder.Entity<CourseTopic>().HasOne(ct => ct.Course).WithMany(c => c.CoursesTopics).HasForeignKey(ct => ct.CourseId);
        modelBuilder.Entity<CourseTopic>().HasOne(ct => ct.Topic).WithMany(t => t.CoursesTopics).HasForeignKey(ct => ct.TopicId);

    }

}