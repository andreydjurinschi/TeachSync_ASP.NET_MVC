using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels.UserViewModels;

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
        /***
         * поиск полльзователя по логину и паролю
         * загрузка связанной сущности Role
         */
        var user = _context.Users.Include(user => user.Role)
            .FirstOrDefault(u => u.Email == email && u.Password == password);
        /***
         * если пользователь не найден, то возвращаем ту же форму с характерным сообщением
         */
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return View();
        }
        /***
         * создается список claims - информация о пользователе, вошедшем в систему
         */
        var claims = new List<Claim>
        {
            // claim с именем
            new Claim(ClaimTypes.Name, user.Name),
            // claim с почтой
            new Claim(ClaimTypes.Email, user.Email),
            // claim c ролью
            new Claim(ClaimTypes.Role, user.Role.Name), 
            // claim c идентификатором
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        // утсанавливаем схему аутентификации и передаем все claims 
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // оболочка, которая передается в систему авторизации
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        // выполняется вход в систему и перенаправление на главную страницу
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult PasswordRecovery()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Password.IsNullOrEmpty() ||  model.Email.IsNullOrEmpty() ||  model.ConfirmPassword.IsNullOrEmpty())
        {
            ModelState.AddModelError(string.Empty, "All fields are required");
            return View(model);
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return View(model);
        }
        
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match");
            return View(model);
        }
        
        user.Password = model.Password;
        _context.Update(user);
        await _context.SaveChangesAsync();
        ModelState.AddModelError(string.Empty, "Password successfully changed");
        return View("Login");
    }


}