using TeachSyncApp.Models;

namespace TeachSyncApp.ViewModels;

public class CourseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? TeacherId { get; set; }
    
    public int? TopicId { get; set; }
    
    public List<User>? TeachersList { get; set; } = new List<User>();
}