using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index action will retrieve a listing of projects (database)
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            // Retrieve all projects from the database
            var projects = _context.Projects.ToList();
            return View(projects);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project); // Add new project to database
                _context.SaveChanges(); // Save changes to database
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            // Retrieves the project with the specified id or returns null if not found
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            // Retrieves the project with the specified id or returns null if not found
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
        {
            // Ensures the id in the route matches the ID in model
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Projects.Update(project); // Update the project
                    _context.SaveChanges(); // Commit changes
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            // Retrieves the project with the specified id or returns null if not found
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        /// <summary>
        /// Handles the confirmation to delete a project.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>Redirects to Index if successful; otherwise, NotFound.</returns>
        [HttpPost("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectId)
        {
            var project = _context.Projects.Find(ProjectId);
            if (project != null)
            {
                _context.Projects.Remove(project); // Removes the project from the database.
                _context.SaveChanges(); // Saves the changes to the database.
                return RedirectToAction("Index");
            }
            return NotFound(); // Returns a 404 if the project doesn't exist.
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = _context.Projects.AsQueryable();

            bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);
            
            if (searchPerformed)
            {
                searchString = searchString.ToLower();
                projectsQuery = projectsQuery.Where(p => p.Name.ToLower().Contains(searchString) ||
                                                         p.Description.ToLower().Contains(searchString));
            }

            // Asynchronous execution means this method does not block the thread while waiting for the database
            var projects = await projectsQuery.ToListAsync();
            return View(projects);
        }
    }
}
