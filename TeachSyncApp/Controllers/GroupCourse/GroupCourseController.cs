using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Controllers.GroupCourse;

public class GroupCourseController : Controller
{
    ApplicationDbContext _context;

    public GroupCourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var groupCourses = await _context.GroupCourses.Include(c => c.Group).Include(c =>c.Course).ToListAsync();
        return View(groupCourses);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var groupCourse = await _context.GroupCourses.Include(c => c.Group).Include(c => c.Course).FirstOrDefaultAsync(m => m.Id == id);
        if (groupCourse == null)
        {
            return NotFound();
        }
        return View(groupCourse);
    }

    [HttpGet]
    public async Task<IActionResult> CreateGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        ViewBag.GroupId = id;
        ViewBag.Courses = new SelectList(_context.Courses, "Id", "Name");
        return View("Create");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(int courseId, int groupId)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.GroupId = groupId;
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "Name");
            return View("Create");
        }
        bool exists = await _context.GroupCourses.AnyAsync(c => c.GroupId == groupId && c.CourseId == courseId);

        if (exists)
        {
            ModelState.AddModelError("", "This group already have this course");
            ViewBag.GroupId = groupId;
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "Name");
            return View("Create");
        }

        var groupCourse = new Models.intermediateModels.GroupCourse()
        {
            GroupId = groupId,
            CourseId = courseId,
        };
        
        _context.Add(groupCourse);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Group");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var groupCourse = await _context.GroupCourses.Include(c => c.Course).Include(c => c.Group).FirstOrDefaultAsync(m => m.Id == id);
        if (groupCourse == null)
        {
            return NotFound();
        }
        return View(groupCourse);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Models.intermediateModels.GroupCourse groupCourse)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "This group already have this course");
            return View(groupCourse);
        }
        bool exists = await _context.GroupCourses.AnyAsync(
            gc => gc.GroupId == groupCourse.GroupId && gc.CourseId == groupCourse.CourseId && gc.Id != id
            );
        if (exists)
        {
            ModelState.AddModelError(" ", "This group already have this course");
        }

        var groupCoursetoUpdate = await _context.GroupCourses.FirstOrDefaultAsync(gc => gc.Id == id);
        if (groupCoursetoUpdate == null)
        {
            return NotFound();
        }
        groupCourse.GroupId = groupCourse.GroupId;
        groupCourse.CourseId = groupCourse.CourseId;
        _context.Update(groupCourse);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


}