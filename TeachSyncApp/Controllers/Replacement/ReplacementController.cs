using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
using TeachSyncApp.Models;
using TeachSyncApp.ViewModels;
using TeachSyncApp.ViewModels.Replacement;

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
        var username = User.Identity?.Name;
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        var replacements = await _context.Replacements
            .Where(r => teacher != null && r.Schedule.TeacherId == teacher.Id)
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
                .ToListAsync(),
            ApprovedByList = new List<Models.User>()
        };
        return replacement;
    }

    [HttpPost]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public async Task<IActionResult> Create(ReplacementViewModel replacementData)
    {
        // проверка валидности данных из формы
        if (!ModelState.IsValid)
        {
            // если данные не валидны,
            return RedirectToAction(nameof(Index));
        }
        // создание модели замены для сохранения в БД
        var replacementToDb = new Models.Replacement
        {
            ScheduleId = replacementData.ScheduleId,
            CourseTopicId = replacementData.CourseTopicId,
            RequestRime = DateTime.Now,
            ApprovedById = replacementData.ApprovedById,
            Status = Status.Pending,
        };
        // добавление замены в контекст и последующее ее сохранение в БД 
        _context.Add(replacementToDb);
        await _context.SaveChangesAsync();
        
        // получаем полную информацию о замене, включая данные о расписании
        var replacement = await _context.Replacements
            .Include(r => r.Schedule)
            .FirstOrDefaultAsync(r => r.Id == replacementToDb.Id);
        
        // если замена успешно найдена, то
        if (replacement != null)
        {
            // в переменные сохраняем данные о начале и конце занятии
            var startTime = replacement.Schedule.EndTime;
            var endTime = replacement.Schedule.EndTime;
            
            
            // получаем свободных учителей у которых время и день занятий не пересекаются с заменой
            var availableTeachers = await _context.Users.Where(u => u.RoleId == 3)
                .Where(u => !u.Schedules.Any(s=> s.StartTime <= endTime && s.EndTime >= startTime && s.DayOfWeekId == replacement.Schedule.DayOfWeekId)).ToListAsync();
            // каждому свободному учителю создаем уведомления
            foreach (var teacher in availableTeachers)
            {
                var notification = new Models.Notification();
                notification.ScheduleId = replacement.ScheduleId;
                notification.TeacherId = teacher.Id;
                notification.ReplacementId = replacement.Id;
                // добавляем уведомление в контекст и сохраняем изменения
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            // если замена не найдена - возвращаем ошибку 404
            return NotFound();
        }
        // переходим на страницу со списком замен
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
    
    // Метод для получения формы с подтверждением замены
    [HttpGet]
    public async Task<IActionResult> GetApproveForm(int? replacementId)
    {
        if (replacementId == null)
            return NotFound();
        // поиск замены по идентификатору с загрузкой всех необходимых сущностей
        var replacement = await _context.Replacements
                // получение расписания
            .Include(r => r.Schedule)
                // получение кабинета, указанного в расписании
            .ThenInclude(s => s.ClassRoom)
                // получение курса и темы курса
            .Include(r => r.CourseTopic)
                // получение курса (для вывода его названия в представлении)
            .ThenInclude(ct => ct.Course)
                // подключение курса и темы курса
            .Include(r => r.CourseTopic)
                // получение темы (для вывода ее названия в представлении)
            .ThenInclude(ct => ct.Topic)
                // поиск замену по указанному Id
            .FirstOrDefaultAsync(r => r.Id == replacementId);
        
        // если замена не найдена, то возвращается ошибка 404
        if (replacement == null)
        {
            return NotFound();
        }
        // возвращаем представление с данными замены
        return View(replacement);
    }
    
    [HttpPost] 
    public async Task<IActionResult> Approve(int replacementId, int teacherId)
    {
        // получаем замену по ID с включением расписания
        var replacement = await _context.Replacements
            .Include(r => r.Schedule)
            .FirstOrDefaultAsync(r => r.Id == replacementId);

        if (replacement == null)
            return NotFound();

        // получаем учителя по ID
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
        if (teacher == null)
        {
            return NotFound();
        }

        // проверяем, не создавался ли уже отклик от этого учителя
        var alreadyResponsed = await _context.ReplacementResponses
            .AnyAsync(r => r.ReplacementId == replacementId && r.TeacherId == teacherId);

        // если отклик отсутствует — создаём новый
        if (!alreadyResponsed)
        {
            var response = new ReplacementResponse
            {
                ReplacementId = replacement.Id,
                TeacherId = teacher.Id,
                Status = Status.Approved,
                ResponsedAt = DateTime.Now
            };

            _context.ReplacementResponses.Add(response);

            // удаляем уведомление, если есть
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.ReplacementId == replacementId && n.TeacherId == teacherId);

            if (notification != null)
                _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();
        }

        // обновляем замену: кто утвердил и статус
        replacement.ApprovedById = teacherId;
        replacement.Status = Status.Approved;

        await _context.SaveChangesAsync();

        // перенаправление на список замен
        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Reject(int? replacementId, int? teacherId)
    {
        if (replacementId == null || teacherId == null)
        {
            return NotFound();
        }
        
        var replacement = await _context.Replacements.FindAsync(replacementId);
        var teacher = await _context.Users.FindAsync(teacherId);

        if (replacement == null || teacher == null)
        {
            return NotFound();
        }
        
        ReplacementResponse replacementResponse  = new ReplacementResponse();
        replacementResponse.ReplacementId = replacement.Id;
        replacementResponse.TeacherId = teacher.Id;
        replacementResponse.Status = Status.Rejected;
        replacementResponse.ResponsedAt = DateTime.Now;
        _context.ReplacementResponses.Add(replacementResponse);
        await _context.SaveChangesAsync();
        
        Models.Notification notification = _context.Notifications
            .FirstOrDefault(n => n.ReplacementId == replacementId && n.TeacherId == teacher.Id)!;
        try
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetForUser", "Notification");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Cannot delete replacement");
            return RedirectToAction(nameof(Index));
        }
    }
    
[HttpGet]
public async Task<IActionResult> GetReplacementsData(string status)
{
    var pendingReplacements = await _context.Replacements
        .Where(r => r.Status == Status.Pending)
        .Include(r => r.Schedule)
            .ThenInclude(s => s.Teacher)
        .Include(r => r.Schedule)
            .ThenInclude(s => s.GroupCourse)
                .ThenInclude(gc => gc.Group)
        .Include(r => r.Schedule)
            .ThenInclude(s => s.GroupCourse)
                .ThenInclude(gc => gc.Course)
        .Include(r => r.Schedule.WeekDays)
        .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Course)
        .Include(r => r.CourseTopic)
            .ThenInclude(ct => ct.Topic)
        .ToListAsync();

    var approvedReplacements = await _context.ReplacementResponses
        .Where(r => r.Status == Status.Approved)
        .Include(r => r.Teacher)
        .Include(r => r.Replacement)
            .ThenInclude(rep => rep.Schedule)
                .ThenInclude(s => s.GroupCourse)
                    .ThenInclude(gc => gc.Group)
        .Include(r => r.Replacement)
            .ThenInclude(rep => rep.Schedule)
                .ThenInclude(s => s.GroupCourse)
                    .ThenInclude(gc => gc.Course)
        .Include(r => r.Replacement!.Schedule.WeekDays)
        .ToListAsync();

    var rejectedReplacements = await _context.ReplacementResponses
        .Where(r => r.Status == Status.Rejected)
        .Include(r => r.Teacher)
        .Include(r => r.Replacement)
            .ThenInclude(rep => rep.Schedule)
                .ThenInclude(s => s.GroupCourse)
                    .ThenInclude(gc => gc.Group)
        .Include(r => r.Replacement)
            .ThenInclude(rep => rep.Schedule)
                .ThenInclude(s => s.GroupCourse)
                    .ThenInclude(gc => gc.Course)
        .Include(r => r.Replacement.Schedule.WeekDays)
        .ToListAsync();
    ViewBag.status
        =  String.IsNullOrEmpty(status) ? status : "";
    var model = new ReplacementStatisticsViewModel();
    switch (status?.ToLower())
    {
        case "pending":
            model.PendingReplacements = pendingReplacements;
            break;
        case "approved":
            model.AppliedReplacements = approvedReplacements;
            break;
        case "rejected":
            model.RejectedReplacements = rejectedReplacements;
            break;
        default:
            model.PendingReplacements = pendingReplacements;
            model.AppliedReplacements = approvedReplacements;
            model.RejectedReplacements = rejectedReplacements;
            break;
    }
    return View(model);
}




}