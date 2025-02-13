using TeachSyncApp.Models;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.ViewModels;

public class ScheduleForm
{
    public ScheduleViewModel scheduleViewModel { get; set; }
    public List<WeekDays> weekDaysList { get; set; }
    public TimeSpan startTime { get; set; }
    public TimeSpan endTime { get; set; }
    public List<ClassRoom> classRoomList { get; set; }
    public List<GroupCourse> groupCourseList { get; set; }
    public List<User> teacherList { get; set; }
    
}