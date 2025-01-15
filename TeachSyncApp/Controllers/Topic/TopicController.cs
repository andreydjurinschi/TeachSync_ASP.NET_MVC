using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachSyncApp.Context;

namespace TeachSyncApp.Controllers.Topic;

public class TopicController : Controller
{
    private ApplicationDbContext _context;

    public TopicController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var topics = await _context.Topics.ToListAsync();
        return View(topics);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Models.Topic newTopic)
    {
        if (!ModelState.IsValid)
        {
            return View(newTopic);
        }

        var topic = new Models.Topic
        {
            Name = newTopic.Name,
        };
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Models.Topic topic)
    {
        if (!ModelState.IsValid)
        {
            return View(topic);
        }

        var topicToUpdate = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id);
        if (topicToUpdate == null)
        {
            return NotFound();
        }

        topicToUpdate.Name = topic.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        try
        {
            _context.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Deleting Topic could not be completed.");
            return RedirectToAction("Index");
        }
    }
}