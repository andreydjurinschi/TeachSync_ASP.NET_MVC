using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.ViewModels;

namespace TeachSyncApp.Controllers.Schedule;

[Authorize]
public class ScheduleController : Controller
{
    private ApplicationDbContext _context;

    public ScheduleController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _context.Users.Where(u => u.RoleId == 3).Include(u => u.Role).ToListAsync();
        return View(teachers);
    }

    [HttpGet]
    public async Task<IActionResult> CreateForTeacher(int teacherId)
    {
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
    
        if (teacher == null)
        {
            return NotFound();
        }

        var scheduleData = await GetScheduleViewModels();

        var filterGroups = _context.GroupCourses.Include(c => c.Course).Include(g => g.Group).Where(c => c.Course.TeacherId == teacher.Id).ToListAsync();
        
        return View(new ScheduleViewModel()
        {
            TeacherId = teacherId, 
            TeacherList = new List<Models.User> { teacher }, 
            DayOfWeekList = scheduleData.DayOfWeekList,
            ClassRoomList = scheduleData.ClassRoomList,
            GroupCourseList = await filterGroups,
        });
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet]
    public async Task<IActionResult> GetSchedulesForTeacher()
    {
        var username = User.Identity?.Name;
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        if (teacher == null)
        {
            return NotFound();
        }

        var schedules = await _context.Schedules
            .Include(schedule => schedule.Teacher)
            .Include(schedule => schedule.WeekDays)
            .Include(schedule => schedule.ClassRoom)
            .Include(schedule => schedule.GroupCourse)
            .ThenInclude(g => g.Group)
            .Include(schedule => schedule.GroupCourse)
            .ThenInclude(g => g.Course).Where(schedule => schedule.TeacherId == teacher.Id).ToListAsync();
        
        return View(schedules);
    }

    
    
    [Authorize(Roles = "Manager")]
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
        if (CheckTime(schedule) == false)
        {
            ModelState.AddModelError("", "Please input time correctly");
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
        if (CheckTime(schedule) == false)
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

    private bool CheckTime(Models.Schedule schedule)
    {
        if (schedule.EndTime < schedule.StartTime)
        {
            return false;
        }

        return true;
    }
}