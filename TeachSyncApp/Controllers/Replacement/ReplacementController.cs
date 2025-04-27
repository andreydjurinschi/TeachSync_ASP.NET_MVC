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
        _context = context;
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

    [HttpPost]
    public async Task<IActionResult> Create(ReplacementViewModel replacementData)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }
        
        var replacementToDb = new Models.Replacement
        {
            ScheduleId = replacementData.ScheduleId,
            CourseTopicId = replacementData.CourseTopicId,
            RequestRime = DateTime.Now,
            ApprovedById = replacementData.ApprovedById,
            Status = Status.Pending,
        };

        _context.Add(replacementToDb);
        await _context.SaveChangesAsync();
        
        var replacement = await _context.Replacements
            .Include(r => r.Schedule)
            .FirstOrDefaultAsync(r => r.Id == replacementToDb.Id);
        if (replacement != null)
        {
            var startTime = replacement.Schedule.EndTime;
            var endTime = replacement.Schedule.EndTime;
            var availableTeachers = await _context.Users.Where(u => u.RoleId == 3)
                .Where(u => !u.Schedules.Any(s=> s.StartTime <= endTime && s.EndTime >= startTime && s.DayOfWeekId == replacement.Schedule.DayOfWeekId)).ToListAsync();
//http://localhost:5239/Replacement/Approve
            foreach (var teacher in availableTeachers)
            {
                var notification = new Models.Notification();
                notification.ScheduleId = replacement.ScheduleId;
                notification.TeacherId = teacher.Id;
                notification.ReplacementId = replacement.Id;
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            return NotFound();
        }


        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> CreateForm(int teacherId, int courseId)
    {
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
        if (teacher == null)
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .Include(s => s.GroupCourse) 
            .FirstOrDefaultAsync(s => s.TeacherId == teacherId && s.GroupCourse.CourseId == courseId);

        var courseTopic = await _context.CoursesTopics.Where(c => schedule != null && c.CourseId == schedule.GroupCourse.CourseId)
            .FirstOrDefaultAsync();
        if (schedule == null)
        {
            return NotFound();
        }

        var replacementData = await GetReplacementViewModel();
        replacementData.SchedulesList = new List<Models.Schedule> { schedule };
        if (courseTopic == null)
        {
            replacementData.CourseTopicsList = new List<Models.intermediateModels.CourseTopic>();
        }
        else
        {
            replacementData.CourseTopicsList = new List<Models.intermediateModels.CourseTopic>
            {
                courseTopic
            };
        }
        
        return View(new ReplacementViewModel()
        {
            ScheduleId = schedule.Id,
            SchedulesList = replacementData.SchedulesList,
            CourseTopicsList = replacementData.CourseTopicsList,
            ApprovedByList = replacementData.ApprovedByList,
            Status = Status.Pending
        });
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var replacements = await GetReplacements();
        return View(replacements);
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
    public async Task<IActionResult> Approve(int? replacementId, int teacherId)
    {
        if (replacementId == null)
        {
            return NotFound();
        }
        var replacement = await _context.Replacements.Include(r => r.Schedule).FirstOrDefaultAsync(r => r.Id == replacementId);
        if (replacement == null)
        {
            return NotFound();
        }
        var startTime = replacement.Schedule.StartTime;
        var endTime = replacement.Schedule.EndTime;
        var availableTeachers = await _context.Users.Where(u => u.RoleId == 3)
            .Where(u => !u.Schedules.Any(s=> s.StartTime <= endTime && s.EndTime >= startTime && s.DayOfWeekId == replacement.Schedule.DayOfWeekId)).ToListAsync();
 
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

}