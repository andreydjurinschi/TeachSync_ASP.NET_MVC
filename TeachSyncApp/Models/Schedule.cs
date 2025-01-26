using System.ComponentModel.DataAnnotations;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Models;

public class Schedule
{
    public int Id { get; set; }
    
    [Required]
    public int DayOfWeekId { get; set; } // FK
    
    public WeekDays WeekDays { get; set; } = null!;
    
    [Required]
    public TimeSpan StartTime { get; set; }
    
    [Required]
    public TimeSpan EndTime { get; set; }
    
    [Required]
    public int ClassRoomId { get; set; } // FK
    public ClassRoom ClassRoom { get; set; } = null!;
    
    [Required]
    public int GroupCourseId { get; set; } //FK
    public GroupCourse GroupCourse { get; set; }= null!;
    
    [Required]
    public int TeacherId { get; set; } // FK

    public User Teacher { get; set; }= null!;
    
    public ICollection<Replacement> Replacements { get; set; } = new List<Replacement>();
}