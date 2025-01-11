using System.ComponentModel.DataAnnotations;
using TeachSyncApp.Models;

namespace TeachSyncApp.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 30 characters")]
    public string Surname { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "User must have role")] 
    [Display(Name = "Role")]
    public int RoleId { get; set; }

}