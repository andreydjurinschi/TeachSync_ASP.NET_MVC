using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels.UserViewModels;

namespace TeachSyncApp.Controllers.Profile;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {   
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        EditProfileViewModel userData = new EditProfileViewModel();
        userData.Name = user!.Name;
        userData.Email = user!.Email;
        userData.Surname = user!.Surname;
        
        return View(userData);
    }
}