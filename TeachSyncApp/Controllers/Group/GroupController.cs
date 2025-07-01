using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Controllers.Group;

public class GroupController : Controller
{
    private ApplicationDbContext _context;

    public GroupController(ApplicationDbContext context)
    {
        _context = context;
    }
    // атрибут, указывающий на обработку GET-запроса
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // извлекаем список всех групп из базы даных с использованием асинхронного метода ToListAsync() 
        var groups = await _context.Groups
        // подключаем связанные сущности GroupCourses (связь между курсами и группами)      
            .Include(g=> g.GroupCourses)
        // подключаем связанные группы для доступа к названию каждой группы
            .ThenInclude(c=> c.Course)
        // подключаем преподавателя, ведущего данный курс
            .ThenInclude(c => c.User)
        // ассинхронное выполнение запроса и преобразование в список
            .ToListAsync();
        // возвращаем представление, передавая ему список групп с загруженными связанными данными
        return View(groups);
    }
    

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        
        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Models.Group newGroup)
    {
        if (!ModelState.IsValid)
        {
            return View(newGroup);
        }
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        group.Name = newGroup.Name;
        group.Year = newGroup.Year;
        group.Capacity = newGroup.Capacity;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Models.Group newGroup)
    {
        if (!ModelState.IsValid)
        {
            return View(newGroup);
        }
        
        var group = new Models.Group
        {
            Name = newGroup.Name,
            Year = newGroup.Year,
            Capacity = newGroup.Capacity
            
        };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        var groupCourse = await _context.GroupCourses.Where(g=> g.GroupId == id).ToListAsync();
        _context.GroupCourses.RemoveRange(groupCourse);
        await _context.SaveChangesAsync();
        try
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException )
        {
            ModelState.AddModelError("", "Can not delete group");
            return RedirectToAction("Index", "Group");
        }
        
    }
}