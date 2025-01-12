namespace TeachSyncApp.Models;

public class Courses
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? TeacherId { get; set; }
    public User? User { get; set; }

}