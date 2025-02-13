using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TeachSyncApp.Context;
using TeachSyncApp.Models;
using TeachSyncApp.ViewModels;
using TeachSyncApp.Controllers;

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

        // Получаем ID курса
        int courseId = getCourseId(replacementData.ScheduleId);
    
        var replacement = new Models.Replacement
        {
            ScheduleId = replacementData.ScheduleId,
            CourseTopicId = courseId, // Используем courseId
            RequestRime = DateTime.Now,
            ApprovedById = replacementData.ApprovedById,
            Status = Status.Pending,
        };

        _context.Add(replacement);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private int getCourseId(int scheduleId)
    {
        // Ищем расписание по ID
        var schedule = _context.Schedules
            .Include(s => s.GroupCourse)  // Необходимо убедиться, что GroupCourse загружен
            .FirstOrDefault(c => c.Id == scheduleId);

        if (schedule == null || schedule.GroupCourse == null)
        {
            throw new Exception("Schedule or GroupCourse not found.");
        }

        // Возвращаем CourseId
        return schedule.GroupCourse.CourseId;
    }

    
    [HttpPost]
    public async Task<IActionResult> GetTopicsBySchedule(int scheduleId)
    {
        // Находим расписание по заданному ScheduleId
        var schedule = await _context.Schedules
            .Include(s => s.GroupCourse)
            .ThenInclude(gc => gc.Course)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null || schedule.GroupCourse == null)
        {
            return NotFound("Schedule or GroupCourse not found.");
        }

        // Получаем курс для выбранного расписания
        var course = schedule.GroupCourse.Course;

        // Получаем топики, связанные с этим курсом через таблицу CoursesTopics
        var courseTopics = await _context.CoursesTopics
            .Where(ct => ct.CourseId == course.Id)
            .Include(ct => ct.Topic) // Загружаем связанные топики
            .ToListAsync();

        // Возвращаем список топиков в формате JSON
        var topics = courseTopics.Select(ct => new
        {
            id = ct.Id,
            courseName = ct.Course.Name,
            topicName = ct.Topic.Name
        }).ToList();

        return Json(topics);
    }




}