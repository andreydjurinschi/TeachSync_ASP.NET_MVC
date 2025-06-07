using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TeachSyncApp.Models.intermediateModels;

namespace TeachSyncApp.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(2018,2025, ErrorMessage = "Please enter an Year between 2018 and 2025")]
    public int Year { get; set; } = DateTime.Now.Year;
    public int Capacity { get; set; }
    public List<GroupCourse> GroupCourses { get; set; } = new List<GroupCourse>();
}