using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(2018,2025, ErrorMessage = "Please enter an Year between 2018 and 2025")]
    public int Year { get; set; } = DateTime.Now.Year;
    public int Capacity { get; set; }
}