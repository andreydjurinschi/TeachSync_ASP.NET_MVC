using System.ComponentModel.DataAnnotations;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Models;

public class Topic
{
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Topic must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Topic cannot be longer than 50 characters.")]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<CourseTopic> CoursesTopics { get; set; } = new HashSet<CourseTopic>();
}