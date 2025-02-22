using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.Models;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Controllers.Replacement;

public class ReplacementController : Controller
{
    private ApplicationDbContext _context;

    public ReplacementController(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));;
    }

    private async Task<List<Models.Replacement>> GetReplacements()
    {
        var replacements = await _context.Replacements
            .Include(r => r.Schedule)
            .ThenInclude(s => s.WeekDays)
            .Include(r => r.Schedule)
            .ThenInclude(s => s.Teacher)
            .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Course)
            .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Topic)
            .Include(r => r.TeacherApprove)
            .ToListAsync();

        return replacements;
    }

    

    private async Task<ReplacementViewModel> GetReplacementViewModel()
    {
        var replacement = new ReplacementViewModel
        {
            SchedulesList = await _context.Schedules
                .Include(s => s.Teacher)
                .Include(s => s.WeekDays)
                .Include(s => s.GroupCourse)
                .ThenInclude(c => c.Group)
                .ToListAsync(),
            CourseTopicsList = await _context.CoursesTopics
                .Include(c => c.Topic)
                .Include(c => c.Course)
                //.ThenInclude(c => c.Name)
                .ToListAsync(),
            ApprovedByList = new List<Models.User>()
        };
        return replacement;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var replacements = await GetReplacements();
        return View(replacements);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var replacementData = await GetReplacementViewModel();
        return View(replacementData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReplacementViewModel replacementData)
    {
        if (!ModelState.IsValid)
        {
            replacementData = await GetReplacementViewModel();
            return View(replacementData);
        }
    
        var replacement = new Models.Replacement
        {
            ScheduleId = replacementData.ScheduleId,
            CourseTopicId = replacementData.CourseTopicId, 
            RequestRime = DateTime.Now,
            ApprovedById = replacementData.ApprovedById,
            Status = Status.Pending,
        };

        _context.Add(replacement);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var replacement = await _context.Replacements
            .Include(r => r.Schedule)
            .ThenInclude(s => s.Teacher)
            .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Course)
            .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Topic)
            .Include(r => r.TeacherApprove)
            .FirstOrDefaultAsync(r => r.Id == id);
        return View(replacement);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var replacement = await _context.Replacements.FindAsync(id);
        if (replacement == null)
        {
            return NotFound();
        }
        try
        {
            _context.Replacements.Remove(replacement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Cannot delete replacement");
            return RedirectToAction(nameof(Index));
        }
    } 
    
        [HttpGet]
    public async Task<IActionResult> Approve(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var replacement = await _context.Replacements.Include(r => r.Schedule).FirstOrDefaultAsync(r => r.Id == id);
        if (replacement == null)
        {
            return NotFound();
        }
        var startTime = replacement.Schedule.StartTime;
        var endTime = replacement.Schedule.EndTime;
        var availableTeachers = await _context.Users.Where(u => u.RoleId == 3)
            .Where(u => !u.Schedules.Any(s=> s.StartTime <= endTime && s.EndTime >= startTime)).ToListAsync();
        if (availableTeachers.Count == 0)
        {
            ModelState.AddModelError("", "No teachers are available to approve");
        }
        ViewBag.AvailableTeachers = availableTeachers;
        return View(replacement);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int replacementId, int teacherId)
    {
        var replacement = await _context.Replacements.Include(r => r.Schedule).FirstOrDefaultAsync(r => r.Id == replacementId);
        if (replacement == null)
        {
            return NotFound();
        }
        var startTime = replacement.Schedule.StartTime;
        var endTime = replacement.Schedule.EndTime;
        
        var teacher = await _context.Users.Include(u => u.Schedules).FirstOrDefaultAsync(u => u.Id == teacherId);
        if (teacher == null || teacher.Schedules.Any(s => s.StartTime <= endTime && s.EndTime >= startTime))
        {
            ModelState.AddModelError("", "No teachers are available to approve"); 
            var availableTeachers = await _context.Users.Where(u => u.RoleId == 3)
                .Where(u => !u.Schedules.Any(s=> s.StartTime <= endTime && s.EndTime >= startTime)).ToListAsync();
            ViewBag.AvailableTeachers = availableTeachers;
            return View(replacement);
        }
        
        replacement.ApprovedById = teacherId;
        replacement.Status = Status.Approved;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    
    
    
    
    
    
    
    /*[HttpGet]
public async Task<IActionResult> Approve(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var replacement = await _context.Replacements
        .Include(r => r.Schedule)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (replacement == null)
    {
        return NotFound();
    }

    var startTime = replacement.Schedule.StartTime;
    var endTime = replacement.Schedule.EndTime;

    
    var availableTeachers = await _context.Users
        .Where(u => u.RoleId == 3)
        .Where(u => !u.Schedules.Any(s => s.StartTime < endTime && s.EndTime > startTime))
        .ToListAsync();

    var replacementViewModel = await GetReplacementViewModel(); 

    
    if (availableTeachers.Count == 0)
    {
        ModelState.AddModelError("", "No teachers are available to approve");
    }

    replacementViewModel.ApprovedByList = availableTeachers;
    
    return View(replacementViewModel);
}

/*[HttpPost]
public async Task<IActionResult> Approve(int replacementId, int teacherId)
{
    var replacement = await _context.Replacements
        .Include(r => r.Schedule)
        .Include(u => u.TeacherApprove)
        .FirstOrDefaultAsync(r => r.Id == replacementId);

    if (replacement == null)
    {
        return NotFound();
    }

    var startTime = replacement.Schedule.StartTime;
    var endTime = replacement.Schedule.EndTime;

    var replacementViewModel = await GetReplacementViewModel();
    var teacher = await _context.Users
        .Include(u => u.Schedules)
        .FirstOrDefaultAsync(u => u.Id == teacherId);

    if (teacher == null || teacher.Schedules.Any(s => s.StartTime < endTime && s.EndTime > startTime))
    {
        ModelState.AddModelError("", "No teachers are available to approve");
        var availableTeachers = await _context.Users
            .Where(u => u.RoleId == 3)
            .Where(u => !u.Schedules.Any(s => s.StartTime < endTime && s.EndTime > startTime))
            .ToListAsync();

        replacementViewModel.ApprovedByList = availableTeachers;
        return View(replacementViewModel);
    }
    
    replacement.ApprovedById = teacherId;
    replacement.Status = Status.Approved;
    await _context.SaveChangesAsync();
    return RedirectToAction("Index", "Home");
}
*/

}