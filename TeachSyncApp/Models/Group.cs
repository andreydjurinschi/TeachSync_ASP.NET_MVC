namespace TeachSyncApp.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; } = DateTime.Now.Year;
    public int Capacity { get; set; }
}