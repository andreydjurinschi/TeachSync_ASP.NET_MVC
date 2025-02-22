using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.Models;
using TeachSyncApp.ViewModels;


namespace TeachSyncApp.Controllers.User;

[Authorize]
public class UserController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchName, string? roleName)
    {
        ViewData["SearchName"] = searchName;
        if (!string.IsNullOrEmpty(searchName))
        {
            var sortedUsers = context.Users.Include(u => u.Role)
                .Where(u => u.Name.ToLower().Contains(searchName.ToLower()) || u.Surname.ToLower().Contains(searchName.ToLower()));
            return View(await sortedUsers.ToListAsync());
        }
        ViewData["RoleName"] = roleName;
        if (!string.IsNullOrEmpty(roleName))
        {
            var sortedByRoles = context.Users.Include(u => u.Role)
                .Where(u => u.Role.Name.ToLower().Contains(roleName.ToLower()));
            return View(await sortedByRoles.ToListAsync());
        }

        var users = context.Users.Include(u => u.Role); 
        return View(await users.ToListAsync());

    }



    [HttpGet]
    public async Task<IActionResult> Details(int? id) 
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
    public async Task<IActionResult> Create()
    {
        var userData = await GetUserViewModel();
        return View(userData);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserViewModel modelUser)
    {
        if(!ModelState.IsValid)
        {
            modelUser = await GetUserViewModel();
            return View(modelUser);
        }

        var user = new Models.User
        {
            Name = modelUser.Name,
            Surname = modelUser.Surname,
            Email = modelUser.Email,
            Password = modelUser.Password,
            RoleId = modelUser.RoleId
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await GetUserById(id);
        var userRoles = await GetUserEditViewModel(); 
        return View(new UserEditViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            RoleId = user.RoleId,
            Roles = userRoles.Roles
        });
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, UserEditViewModel modelUser)
    {
        if(!ModelState.IsValid)
        {
            modelUser = await GetUserEditViewModel(); 
            return View(modelUser);
        }
        var user = await GetUserById(id);
        
        user.Name = modelUser.Name;
        user.Surname = modelUser.Surname;
        user.Email = modelUser.Email;
        user.RoleId = modelUser.RoleId;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var user = await GetUserById(id);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await GetUserById(id);
        try
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "User could not be deleted");
            return RedirectToAction("Index", "User");
        }
    }

    private async Task<UserViewModel> GetUserViewModel()
    {
        var user = new UserViewModel();
        user.Roles = await context.Roles.ToListAsync();
        return user;
    }

    private async Task<UserViewModel> GetUserEditViewModel()
    {
        var user = new UserViewModel();
        user.Roles = await context.Roles.ToListAsync();
        return user;
    }
    
    private async Task<Models.User> GetUserById(int? id)
    {
        return (await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id))!;
    }









}