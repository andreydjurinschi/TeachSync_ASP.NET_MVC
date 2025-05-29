namespace TeachSyncApp.ViewModels.UserViewModels;

public class ProfileViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
}