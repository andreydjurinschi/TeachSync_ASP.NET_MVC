using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models;

public class User
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
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "User must have role")] 
    [Display(Name = "Role")]
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;// 1 роль у 1 пользователя
    public ICollection<Courses> Courses { get; set; } = new List<Courses>();
}
