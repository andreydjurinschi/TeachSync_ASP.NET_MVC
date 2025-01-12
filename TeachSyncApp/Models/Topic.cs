using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models;

public class Topic
{
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public string Name { get; set; } = string.Empty;
}