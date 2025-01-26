using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models;

public class ClassRoom
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter the name of the room")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please enter the capacity of the room")]
    public int Capacity { get; set; }
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    
}