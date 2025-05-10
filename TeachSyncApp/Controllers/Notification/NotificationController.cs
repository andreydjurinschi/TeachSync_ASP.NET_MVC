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
        return View(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> DropNotification(int notificationId)
    {
        int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var notification = _context.Notifications.FirstOrDefault(
            n => n.TeacherId == teacherId
            && n.Id == notificationId
        );

        if (notification == null)
        {
            return NotFound();
        }
        try
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            var replacementId = notification.ReplacementId;
            var otherNotifications = await _context.Notifications.Where(n => n.ReplacementId == replacementId).ToListAsync();
            if (otherNotifications.Count == 0)
            {
                var replacement = await _context.Replacements.Where(r => r.Id == replacementId).FirstOrDefaultAsync();
                if (replacement != null)
                {
                    _context.Replacements.Remove(replacement);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("GetForUser");
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
    }
}