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


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetSchedules());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var scheduleData = await GetScheduleViewModels();
        return View(scheduleData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduleViewModel scheduleData)
    {
        if (!ModelState.IsValid)
        {
            scheduleData = await GetScheduleViewModels();
            return View(scheduleData);
        }

        var schedule = new Models.Schedule()
        {
            DayOfWeekId = scheduleData.DayOfWeekId,
            TeacherId = scheduleData.TeacherId,
            ClassRoomId = scheduleData.ClassRoomId,
            GroupCourseId = scheduleData.GroupCourseId,
            StartTime = scheduleData.StartTime,
            EndTime = scheduleData.EndTime,
        };
        if (checkTime(schedule) == false)
        {
            ModelState.AddModelError("", "Please input data correctly");
            scheduleData = await GetScheduleViewModels();
            return View(scheduleData);
        }
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var schedule = await GetScheduleById(id);
        var scheduleData = await GetScheduleViewModels();
        return View(new ScheduleViewModel()
        {
            Id = schedule.Id,
            DayOfWeekId = schedule.DayOfWeekId,
            TeacherId = schedule.TeacherId,
            ClassRoomId = schedule.ClassRoomId,
            GroupCourseId = schedule.GroupCourseId,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            TeacherList = scheduleData.TeacherList,
            ClassRoomList = scheduleData.ClassRoomList,
            GroupCourseList = scheduleData.GroupCourseList,
            DayOfWeekList = scheduleData.DayOfWeekList,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ScheduleViewModel scheduleData)
    {
        if (!ModelState.IsValid)
        {
            scheduleData = await GetScheduleViewModels();
            return View(scheduleData);
        }
        var schedule = await GetScheduleById(id);
        schedule.DayOfWeekId = scheduleData.DayOfWeekId;
        schedule.TeacherId = scheduleData.TeacherId;
        schedule.ClassRoomId = scheduleData.ClassRoomId;
        schedule.GroupCourseId = scheduleData.GroupCourseId;
        schedule.StartTime = scheduleData.StartTime;
        schedule.EndTime = scheduleData.EndTime;
        if (checkTime(schedule) == false)
        {
            ModelState.AddModelError("", "Please input data correctly");
            scheduleData = await GetScheduleViewModels();
            return View(scheduleData);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private async Task<List<Models.Schedule>> GetSchedules()
    {
        var schedules = await _context.Schedules
            .Include(schedule => schedule.Teacher)
            .Include(schedule => schedule.WeekDays)
            .Include(schedule => schedule.ClassRoom)
            .Include(schedule => schedule.GroupCourse)
            .ThenInclude(g => g.Group)
            .Include(schedule => schedule.GroupCourse)
            .ThenInclude(g => g.Course)
            .ToListAsync();
        return schedules;
    }

    private async Task<Models.Schedule> GetScheduleById(int? id)
    {
        return (await _context.Schedules
            .Include(s => s.Teacher)
            .Include(s => s.ClassRoom)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Group)
            .Include(s => s.GroupCourse)
            .ThenInclude(g => g.Course)
            .FirstOrDefaultAsync(s => s.Id == id))!;
        }

    private async Task<ScheduleViewModel> GetScheduleViewModels()
    {
        var schedule = new ScheduleViewModel();
        schedule.DayOfWeekList = await _context.DaysOfWeek.ToListAsync();
        schedule.TeacherList = await _context.Users.Where(t => t.RoleId == 3).ToListAsync();
        schedule.ClassRoomList = await _context.ClassRooms.ToListAsync();
        schedule.GroupCourseList = await _context.GroupCourses
            .Include(g => g.Group)
            .Include(g => g.Course)
            .ToListAsync();
        return schedule;
    }

    private bool checkTime(Models.Schedule schedule)
    {
        if (schedule.EndTime < schedule.StartTime)
        {
            return false;
        }
        return true;
    }


}












/*private ApplicationDbContext _context;

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
    }*/

