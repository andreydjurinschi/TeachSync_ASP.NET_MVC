using TeachSyncApp.Models;

namespace TeachSyncApp.ViewModels;

public class ReplacementViewModel
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public int CourseTopicId { get; set; }
    public DateTime RequestRime { get; set; }
    public int? ApprovedById { get; set; }
    public Status Status { get; set; }
}