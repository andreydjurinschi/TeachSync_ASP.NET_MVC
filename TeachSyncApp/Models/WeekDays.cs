namespace TeachSyncApp.Models;

public class WeekDays
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}