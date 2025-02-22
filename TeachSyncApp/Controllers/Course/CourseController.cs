using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.Models;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Controllers.Course;

public class CourseController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var courses = await GetCourses();
        return View(courses); 
    }

    private async Task<List<Courses>> GetCourses()
    {
        var courses = await context.Courses.Include(c => c.User)
            .Include(c => c.CoursesTopics).ThenInclude(c=> c.Topic)
            .ToListAsync();
        return courses;
    }

    private async Task<CourseViewModel> GetCourseViewModel() {
        var courseViewModel = new CourseViewModel
        {
            TeachersList = await context.Users.Where(u => u.RoleId == 3).ToListAsync(),
        };
        return courseViewModel;
    }

    private async Task<Courses?> GetCourse(int courseId)
    {
        var course = await context.Courses
            .Include(c => c.User)
            .Include(c => c.CoursesTopics)
            .ThenInclude(ct => ct.Topic)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        return course;
    }

    public async Task<IActionResult> Details(int id)
    {
        var course =await  GetCourse(id);
        return View(course);  
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var courseViewModel = await GetCourseViewModel();
        
        return View(courseViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int? id, CourseViewModel courseModel)
    {
        if (!ModelState.IsValid)
        {
            courseModel = await GetCourseViewModel();
            return View(courseModel);
        }

        var course = new Courses
        {
            Name = courseModel.Name,
            Description = courseModel.Description,
            TeacherId = courseModel.TeacherId,
        };
        context.Courses.Add(course);
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
        var course = await  context.Courses.Include(c => c.User).Where(c => c.Id == id).FirstOrDefaultAsync();
        if (course == null)
        {
            return NotFound();
        }
        var teacherData = await GetCourseViewModel();
        return View(new CourseViewModel
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            TeacherId = course.TeacherId,
            TeachersList = teacherData.TeachersList,
        });
    }

    [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseViewModel courseModel)
        {
            if (!ModelState.IsValid)
            {
                courseModel = await GetCourseViewModel();
                return View(courseModel);
            }
            var course = await context.Courses.Include(c => c.User).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return NotFound();
            }
            course.Name = courseModel.Name;
            course.Description = courseModel.Description;
            course.TeacherId = courseModel.TeacherId;
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
        var course = await context.Courses.Include(c => c.User).Where(c => c.Id == id).FirstOrDefaultAsync();
        if (course == null)
        {
            return NotFound();
        }
        return View(course);
    }

    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Course cannot be deleted");
            return RedirectToAction("Index");
        }
    }

}
