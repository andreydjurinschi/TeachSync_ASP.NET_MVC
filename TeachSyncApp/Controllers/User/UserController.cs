using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;


namespace TeachSyncApp.Controllers.User;

public class UserController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = context.Users.Include(u => u.Role);
        return View(await users.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id) //получаю только 1 пользователя
    {
        if (id == null)
        {
            return NotFound();
        }
        var user =  await context.Users.Include(r => r.Role).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
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
    public async Task<IActionResult> Add([Bind("Name", "Surname", "Email", "RoleId")] Models.User user)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            // Логируем ошибки или выводим их для отладки
            Console.WriteLine(string.Join(", ", errors));

            ViewBag.RoleId = new SelectList(context.Roles, "Id", "Name", user.RoleId);
            return View("Create", user);
        }

        user.CreatedAt = DateTime.UtcNow;
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }







}