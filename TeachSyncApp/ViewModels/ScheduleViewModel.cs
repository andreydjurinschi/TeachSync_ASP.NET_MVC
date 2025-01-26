namespace TeachSyncApp.ViewModels;

public class ScheduleViewModel
{
    public int Id { get; set; }
    public int DayOfWeekId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int ClassRoomId { get; set; }
    public int GroupCourseId { get; set; } 
    public int TeacherId { get; set; } 
}