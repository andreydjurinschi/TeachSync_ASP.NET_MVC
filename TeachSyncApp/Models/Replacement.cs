using System.ComponentModel.DataAnnotations;
using TeachSyncApp.Models.intermediateModels;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Models;

public class Replacement
{
    public int Id { get; set; }
    [Required]
    public int ScheduleId { get; set; }

    public Schedule Schedule { get; set; } 
    [Required]
    public int CourseTopicId { get; set; }
    public CourseTopic CourseTopic { get; set; }
    public DateTime RequestRime { get; set; }
    public int? ApprovedById { get; set; }
    public User? TeacherApprove { get; set; }
    public Status Status { get; set; }
}
public enum Status
{
    Pending,
    Approved,
}