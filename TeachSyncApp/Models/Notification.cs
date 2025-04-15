namespace TeachSyncApp.Models;

public class Notification
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int ReplacementId { get; set; }
    public int ScheduleId { get; set; }  
        
    public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    public User Teacher { get; set; }
    public Replacement Replacement { get; set; }
    public Schedule Schedule { get; set; } 
}