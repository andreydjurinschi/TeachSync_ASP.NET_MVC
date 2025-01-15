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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        
        var groups = await _context.Groups.ToListAsync();
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

        try
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException )
        {
            ModelState.AddModelError("", "Can not delete group");
            return View(group);
        }
        
    }
}