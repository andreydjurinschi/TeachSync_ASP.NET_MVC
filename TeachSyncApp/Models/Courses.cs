using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Models;

public class Courses
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? TeacherId { get; set; }
    public User? User { get; set; }
    
    public ICollection<CourseTopic> CoursesTopics { get; set; } = new HashSet<CourseTopic>();
    public ICollection<GroupCourse> GroupCourses { get; set; } = new HashSet<GroupCourse>();

}