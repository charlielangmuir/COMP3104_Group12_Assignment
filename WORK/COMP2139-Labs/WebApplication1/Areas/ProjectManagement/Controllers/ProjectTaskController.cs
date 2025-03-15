using WebApplication1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Areas.ProjectManagement.Controllers;
[Area("ProjectManagement")]
[Route("ProjectTask")]
public class ProjectTaskController: Controller
{
    public readonly ApplicationDbContext _context;

    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("Index/{projectId:int}")]
    public IActionResult Index(int projectId)
    {
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }
    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var task = await _context.
            Tasks.
            Include(t=>t.Project).
            FirstOrDefaultAsync(t=>t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
            
        }
        return View(task);
    }
    [HttpGet("Create/{projectId:int}")]
    public async Task<IActionResult> Create(int projectId)
    {
        var projects = await _context.Projects.FindAsync(projectId);
        if (projects == null)
        {
            return NotFound();
        }

        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
        };
        return View(task);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return View(task);
    }
    
    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.Include(t=>t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
        
    }

    [HttpGet("Delete/{id:int}")] 
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.Include(t=>t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
            
        }
        
        return View(task);
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int projectTaskId)
    {
        var task = await _context.Projects.FindAsync(projectTaskId);
        if (task != null)
        {
            _context.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return View(task);
    }
    
    [HttpGet("Search")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        var tasksQuery = _context.Tasks.AsQueryable();
        if (projectId.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
        }
        bool searchPerformd = !string.IsNullOrWhiteSpace(searchString);
        if (searchPerformd)
        {
            searchString = searchString.ToLower();
            tasksQuery = tasksQuery.Where(p => p.Title.ToLower().Contains(searchString)|| 
                                                     p.Description.ToLower().Contains(searchString));
        }
        var tasks = await tasksQuery.ToListAsync();
        
        ViewBag.ProjectId = projectId;
        ViewData["SearchString"] = searchString;
        ViewData["SearchPerformed"] = searchPerformd;
        
        return View("Index",tasks);
    }
    
    
}