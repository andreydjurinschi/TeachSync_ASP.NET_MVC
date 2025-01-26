using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    private void ViewBags()
    {
        ViewBag.Teachers = new SelectList(
            _context.Users.Where(u => u.RoleId == 3).Select(
                u => new { u.Id, FullName = u.Name + " " + u.Surname }
            ),
            "Id", "FullName", null
        );
        ViewBag.Schedules = new SelectList(
            _context.Schedules.Select(
                s => new { s.Id, Name = s.GroupCourse.Group.Name + " " + s.GroupCourse.Course.Name }
            ),
            "Id", "Name"
        );
        ViewBag.CourseTopics = new SelectList(_context.CoursesTopics.Select(
                cc => new { cc.Id, Name = cc.Topic.Name }
            ),
            "Id", "Name"
        );
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var replacements = await _context.Replacements
            .Include(r => r.Schedule)
            .Include(r => r.TeacherApprove)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Topic)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Course)
            .ToListAsync();
        return View(replacements);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var replacement = await _context.Replacements.FirstOrDefaultAsync(r => r.Id == id);
        if (replacement == null)
        {
            return NotFound();
        }

        return View(replacement);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var replacement = await _context.Replacements
            .Include(r => r.Schedule)
            .Include(r => r.TeacherApprove)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Topic)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Course)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (replacement == null)
        {
            return NotFound();
        }
        ViewBags();
        var replacementVm = new ReplacementViewModel
        {
            Id = replacement.Id,
            ApprovedById = replacement.ApprovedById,
            CourseTopicId = replacement.CourseTopicId,
            RequestRime = replacement.RequestRime,
            ScheduleId = replacement.ScheduleId,
            Status = replacement.Status
        };
        return View(replacementVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReplacementViewModel replacementViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBags();
            return View(replacementViewModel);
        }
        var replacementToEdit = await _context.Replacements
            .Include(r => r.Schedule)
            .Include(r => r.TeacherApprove)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Topic)
            .Include(r => r.CourseTopic)
            .ThenInclude(c => c.Course)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (replacementToEdit == null)
        {
            ViewBags();
            return View(replacementToEdit);
        }
        replacementToEdit.ApprovedById = replacementViewModel.ApprovedById;
        replacementToEdit.CourseTopicId = replacementViewModel.CourseTopicId;
        replacementToEdit.RequestRime = DateTime.Now;
        replacementToEdit.ScheduleId = replacementViewModel.ScheduleId;
        if (replacementViewModel.ApprovedById == null)
        {
            replacementToEdit.Status = Status.Pending;
        }
        else
        {
            replacementToEdit.Status = Status.Approved;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBags();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReplacementViewModel replacementViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBags();
            return View(replacementViewModel);
        }

        var replacement = new Models.Replacement
        {
            ApprovedById = replacementViewModel.ApprovedById,
            CourseTopicId = replacementViewModel.CourseTopicId,
            RequestRime = DateTime.Now,
            ScheduleId = replacementViewModel.ScheduleId,
            Status = Status.Pending,
        };
            _context.Replacements.Add(replacement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
    }

}