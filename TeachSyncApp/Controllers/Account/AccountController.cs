using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Controllers.Account;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        
        var user = _context.Users.Include(user => user.Role)
            .FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return View();
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name) 
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}