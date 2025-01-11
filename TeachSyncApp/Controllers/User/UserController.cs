using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels;


namespace TeachSyncApp.Controllers.User;

public class UserController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchName)
    {
        ViewData["SearchName"] = searchName;
        if (string.IsNullOrEmpty(searchName))
        {
            var users = context.Users.Include(u => u.Role);
            return View(await users.ToListAsync());
        }
        var sortedUsers = context.Users.Include(u => u.Role)
            .Where(u => u.Name.ToLower().Contains(searchName.ToLower()) || u.Surname.ToLower().Contains(searchName.ToLower()));
        return View(await sortedUsers.ToListAsync());
    }



    [HttpGet]
    public async Task<IActionResult> Details(int? id) //получаю только 1 пользователя
    {
        if (id == null)
        {
            return NotFound();
        }
        var user =  await context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.RoleId = new SelectList(context.Roles, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserViewModel modelUser)
    {
        if(!ModelState.IsValid)
        {
            ViewBag.RoleId = new SelectList(context.Roles, "Id", "Name");
            return View(modelUser);
        }

        var user = new Models.User
        {
            Name = modelUser.Name,
            Surname = modelUser.Surname,
            Email = modelUser.Email,
            RoleId = modelUser.RoleId
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int? id, UserViewModel modelUser)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        if(!ModelState.IsValid)
        {
            return View(modelUser);
        }
        user.Name = modelUser.Name;
        user.Surname = modelUser.Surname;
        user.Email = modelUser.Email;
        user.RoleId = modelUser.RoleId;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

 







}