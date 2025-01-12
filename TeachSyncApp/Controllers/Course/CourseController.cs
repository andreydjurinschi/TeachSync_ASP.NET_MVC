using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Controllers.Course;

public class CourseController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var courses = await context.Courses.Include(c => c.User).ToListAsync();

        return View(courses); // передаем список Courses
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var course = await context.Courses.Include(c => c.User).Where(c => c.Id == id).FirstOrDefaultAsync();
        if (course == null)
        {
            return NotFound();
        }
        return View(course);
    }

    public  Task<IActionResult> Create()
    {
        return View();
    }

    public async Task<IActionResult> Create(int? id, CourseViewModel courseModel)
    {
        
    }
}