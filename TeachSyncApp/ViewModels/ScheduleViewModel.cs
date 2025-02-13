using System.ComponentModel.DataAnnotations;
using TeachSyncApp.Models;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.ViewModels;

public class ScheduleViewModel
{
    public int Id { get; set; }
    
    [Required]
    public int DayOfWeekId { get; set; } // FK

    public List<WeekDays> DayOfWeekList { get; set; } = new List<WeekDays>();
    
    [Required]
    public TimeSpan StartTime { get; set; }
    
    [Required]
    public TimeSpan EndTime { get; set; }
    
    [Required]
    public int ClassRoomId { get; set; } 
    public List<ClassRoom> ClassRoomList { get; set; } = new List<ClassRoom>();
    
    [Required]
    public int GroupCourseId { get; set; } //FK
    public List<GroupCourse> GroupCourseList { get; set; } =  new List<GroupCourse>();
    
    [Required]
    public int TeacherId { get; set; } // FK

    public List<User> TeacherList { get; set; } = new List<User>();
}