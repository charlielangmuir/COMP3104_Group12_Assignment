using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data;


namespace WebApplication1.Areas.ProjectManagement.Controllers;
[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")] 
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.StartDate = project.StartDate.ToUniversalTime();  
                project.EndDate = project.EndDate.ToUniversalTime();
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                //Persist new Project to the database
                return RedirectToAction("Index");
            }
    
            return View(project);
    
    
        }
        [HttpGet]
        [Route("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
     {
         //Database: Retrieve project from  database
         var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
         if (project == null)
         {
             return NotFound();
         }

         return View(project);

     }
        
    [HttpGet]
    [Route("Edit/{id:int}")]
     public async Task<IActionResult> Edit(int id)
     {
         //Returns project or null
         var project = await _context.Projects.FindAsync(id);
         if (project == null)
         {
             return NotFound();
         }

         return View(project);
     }
     
     
     [HttpPost]
     [Route("Edit/{id:int}")] 
     public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name, Description")] Project project)
     {
         if (id != project.ProjectId)
         {
             return NotFound();
         }

         if (ModelState.IsValid)
         {
             try
             {
                 _context.Projects.Update(project);
                 await _context.SaveChangesAsync();

             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!await projectExits(project.ProjectId))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }
         }

         return View(project);
     }
     
     
private async Task<bool> projectExits(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }

    [HttpGet]
    [Route("Delete/{id:int}")] 
    public async Task<IActionResult> Delete(int id)
    {
        //Returns project or null
        var project = await _context.Projects.FirstOrDefaultAsync(p=>p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Route("Delete/{id:int}")] 
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return NotFound();
    }

    [HttpGet("Search/{searchString}")]
    public async Task<IActionResult> Search(string searchString)
    {
        var projectsQuery = _context.Projects.AsQueryable();
        bool searchPerformd = !string.IsNullOrWhiteSpace(searchString);
        if (searchPerformd)
        {
            searchString = searchString.ToLower();
            projectsQuery = projectsQuery.Where(p => p.Name.ToLower().Contains(searchString)|| 
                                                     p.Description.ToLower().Contains(searchString));
        }
        var projects = await projectsQuery.ToListAsync();
        
        ViewData["SearchString"] = searchString;
        ViewData["SearchPerformed"] = searchPerformd;
        
        return View("Index",projects);
    }
    
}