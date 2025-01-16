using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models.intermediateModels;

public class CourseTopic
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter a course name")]
    public int CourseId { get; set; }
    public Courses Course { get; set; } = null!;
    [Required(ErrorMessage = "Please enter a topic name")]
    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;
}