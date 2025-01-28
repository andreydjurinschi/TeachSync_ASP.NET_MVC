using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Controllers.Schedule;

public class ScheduleController : Controller
{
    private ApplicationDbContext _context;

    public ScheduleController(ApplicationDbContext context)
    {
        _context = context;
    }

    private void ViewBags()
    {
        
        ViewBag.Users = new SelectList(
            _context.Users.Where(u => u.RoleId == 3).Select(u => new
                { u.Id, FullName = u.Name + " " + u.Surname }
            ),
            "Id", "FullName", null
        );
        ViewBag.WeekDay = new SelectList(_context.DaysOfWeek, "Id", "Name");
        ViewBag.ClassRoom = new SelectList(_context.ClassRooms, "Id", "Name");
        ViewBag.GroupCourse = new SelectList(_context.GroupCourses
                .Include(c => c.Course)
                .Include(g => g.Group)
                .Select(gc => new
                {
                    gc.Id,
                    DisplayName = gc.Group.Name + " - " + gc.Course.Name
                }),
            "Id", "DisplayName"
        );
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var schedule = _context.Schedules
            .Include(s => s.Teacher)
            .Include(s => s.ClassRoom)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Group)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Course)
            .Include(s => s.WeekDays);
        return View(await schedule.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBags();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduleViewModel scheduleViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBags();
            return View(scheduleViewModel);
        }

        var schedule = new Models.Schedule
        {
            DayOfWeekId = scheduleViewModel.DayOfWeekId,
            ClassRoomId = scheduleViewModel.ClassRoomId,
            GroupCourseId = scheduleViewModel.GroupCourseId,
            TeacherId = scheduleViewModel.TeacherId,
            StartTime = scheduleViewModel.StartTime,
            EndTime = scheduleViewModel.EndTime,
        };
        _context.Schedules.Add(schedule);
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

        var sc = await _context.Schedules.FirstOrDefaultAsync(sc => sc.Id == id);
        if (sc == null)
        {
            return NotFound();
        }
        return View(sc);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var sc = await _context.Schedules.FirstOrDefaultAsync(sc => sc.Id == id);
        if (sc == null)
        {
            return NotFound();
        }

        try
        {
            _context.Schedules.Remove(sc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Schedule cannot be deleted");
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .Include(s => s.Teacher)
            .Include(s => s.ClassRoom)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Group)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Course).FirstOrDefaultAsync(sc => sc.Id == id);
        if (schedule == null)
        {
            return NotFound();
        }
        ViewBags();
        return View(new ScheduleViewModel
        {
            Id = schedule.Id,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            ClassRoomId = schedule.ClassRoomId,
            GroupCourseId = schedule.GroupCourseId,
            TeacherId = schedule.TeacherId,
            DayOfWeekId = schedule.DayOfWeekId
        });
    }

    [HttpPost]
        public async Task<IActionResult> Edit(int id,ScheduleViewModel scheduleViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBags();
                return View(scheduleViewModel);
            }
            var schedule = await _context.Schedules
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .Include(s => s.GroupCourse)
                .ThenInclude(g => g.Group)
                .Include(s => s.GroupCourse)
                .ThenInclude(g => g.Course).FirstOrDefaultAsync(sc => sc.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            schedule.DayOfWeekId = scheduleViewModel.DayOfWeekId;
            schedule.StartTime = scheduleViewModel.StartTime;
            schedule.EndTime = scheduleViewModel.EndTime;
            schedule.ClassRoomId = scheduleViewModel.ClassRoomId;
            schedule.GroupCourseId = scheduleViewModel.GroupCourseId;
            schedule.TeacherId = scheduleViewModel.TeacherId;
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
}
