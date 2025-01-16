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
        {
            return BadRequest("Course ID is required");
        }

        ViewBag.CourseId = id.Value; 
        ViewBag.Topics = new SelectList(_context.Topics, "Id", "Name"); 
    
        return View("Create");
    }


    [HttpPost]
    public async Task<IActionResult> CreatePost(int topicId, int courseId)
    {
        if (!ModelState.IsValid)
        {

            ViewBag.CourseId = courseId;
            ViewBag.Topics = new SelectList(_context.Topics, "Id", "Name");
            return View("Create");
        }
        
        bool exists = await _context.CoursesTopics
            .AnyAsync(ct => ct.CourseId == courseId && ct.TopicId == topicId);

        if (exists)
        {
            ModelState.AddModelError("", "Course Topic already exists");
            ViewBag.CourseId = courseId;
            ViewBag.Topics = new SelectList(_context.Topics, "Id", "Name");
            return View("Create");
        }

        var courseTopic = new Models.intermediateModels.CourseTopic
        {
            CourseId = courseId,
            TopicId = topicId
        };
        _context.Add(courseTopic);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Course");
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