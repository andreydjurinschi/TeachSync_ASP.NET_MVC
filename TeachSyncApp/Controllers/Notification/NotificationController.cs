using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TeachSyncApp.Context;

namespace TeachSyncApp.Controllers.Notification;

public class NotificationController : Controller
{
    private ApplicationDbContext _context;

    public NotificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetForUser()
    {
        int teacherId =  int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        
        var notifications = await _context.Notifications
            .Include(n => n.Schedule)
            .ThenInclude(n => n.Teacher)
            .Include(n => n.Schedule)
            .ThenInclude(n => n.WeekDays)
            .Include(n => n.Schedule)
            .ThenInclude(s => s.GroupCourse)
            .ThenInclude(s => s.Course)
            .Include(n => n.Schedule)
            .ThenInclude(s => s.GroupCourse)
            .ThenInclude(s => s.Group)
            .Include(n => n.Replacement)
            .ThenInclude(r => r.CourseTopic)
            .ThenInclude(c => c.Topic)
            .Include(n => n.Teacher)
            .Where(n => n.TeacherId == teacherId)
            .ToListAsync();
        ViewData["notificationsCount"] = notifications.Count;
        return View(notifications);
    }

}