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
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        var userData = new ProfileViewModel
        {
            Name = user!.Name,
            Email = user!.Email,
            Surname = user!.Surname,
            CreatedAt = user!.CreatedAt,
            Password = user!.Password,
            Role = user!.RoleId,
        };
        return View(userData);
    }

    public IActionResult EditProfile(string? name, string? email, string? surname, string? password)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(name))
        {
            user.Name = name;
        }

        if (!string.IsNullOrEmpty(email))
        {
            user.Email = email;
        }

        if (!string.IsNullOrEmpty(surname))
        {
            user.Surname = surname;
        }

        if (!string.IsNullOrEmpty(password))
        {
            user.Password = password;
        }
        
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}