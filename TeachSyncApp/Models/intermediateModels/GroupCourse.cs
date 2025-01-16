using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models.intermediateModels;

public class GroupCourse
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter a course name")]
    public int GroupId { get; set; }

    public Group Group { get; set; }
    [Required(ErrorMessage = "Please enter a course number")]
    public int CourseId { get; set; }

    public Courses Course { get; set; }


}