using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;
namespace TeachSyncApp.Controllers.CourseTopic;
public class CourseTopicController : Controller
{
    private ApplicationDbContext _context;

    public CourseTopicController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var coursesTopics = await _context.CoursesTopics.Include(c => c.Topic).Include(c => c.Course).ToListAsync();
        return View(coursesTopics);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var courseTopic = await _context.CoursesTopics.Include(c => c.Topic).Include(c => c.Course).FirstOrDefaultAsync(c => c.Id == id);
        if (courseTopic == null)
        {
            return NotFound();
        }
        return View(courseTopic);
    }

    [HttpGet]
    public IActionResult CreateGet(int? id)
    {
        if (id == null)
            return BadRequest("Course ID is required");

        ViewBag.CourseId = id.Value;

        // разделение по Id
        ViewBag.InformaticsTopics = _context.Topics
            .Where(t => t.Id < 21)
            .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
            .ToList();

        ViewBag.DesignTopics = _context.Topics
            .Where(t => t.Id >= 21)
            .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
            .ToList();

        return View("Create");
    }



    [HttpPost]
    public async Task<IActionResult> CreatePost(int courseId, int[] selectedTopicIds)
    {
        if(selectedTopicIds == null || selectedTopicIds.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please select at least one topic.");
            ViewBag.CourseId = courseId;
            ViewBag.Topics = new SelectList(_context.Topics, "Id", "Name");
            return RedirectToAction("Index", "Course");
        }

        foreach(var topicId in selectedTopicIds)
        {
            var courseTopic = new Models.intermediateModels.CourseTopic
            {
                CourseId = courseId,
                TopicId = topicId
            };
            _context.CoursesTopics.Add(courseTopic);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Course", new { id = courseId });
    }


    
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var courseTopic = await _context.CoursesTopics.FirstOrDefaultAsync(c => c.Id == id);
        if (courseTopic == null)
        {
            return NotFound();
        }
        return View(courseTopic);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Models.intermediateModels.CourseTopic courseTopicToUpdate)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Error");
        }
        
        bool exists = await _context.CoursesTopics
            .AnyAsync(ct => ct.CourseId == courseTopicToUpdate.CourseId && ct.TopicId == courseTopicToUpdate.TopicId && ct.Id != id);

        if (exists)
        {
            ModelState.AddModelError("", "This course-topic relation already exists.");
            return View(courseTopicToUpdate);
        }
        
        var courseTopic = await _context.CoursesTopics.FirstOrDefaultAsync(c => c.Id == id);
        if (courseTopic == null)
        {
            return NotFound();
        }
        
        courseTopic.TopicId = courseTopicToUpdate.TopicId;
        courseTopic.CourseId = courseTopicToUpdate.CourseId;
        
        _context.Update(courseTopic);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    

}