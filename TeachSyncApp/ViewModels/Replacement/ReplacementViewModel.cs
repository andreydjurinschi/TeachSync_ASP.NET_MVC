using TeachSyncApp.Models;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.ViewModels;

public class ReplacementViewModel
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public List<Schedule> SchedulesList { get; set; } = new List<Schedule>();
    public int CourseTopicId { get; set; }
    public List<CourseTopic> CourseTopicsList { get; set; } = new List<CourseTopic>();
    public DateTime RequestRime { get; set; }
    public int? ApprovedById { get; set; }
    public List<User>? ApprovedByList { get; set; } = new List<User>();
    public Status Status { get; set; }
}