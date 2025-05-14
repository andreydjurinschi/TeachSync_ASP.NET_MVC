namespace TeachSyncApp.Models;

public class ReplacementResponse
{
    public int Id  { get; set; }
    public int ReplacementId { get; set; }
    public Replacement? Replacement { get; set; }
    public int TeacherId { get; set; }
    public User? Teacher { get; set; }
    public Status Status { get; set; }
    public DateTime ResponsedAt { get; set; }
}