using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Areas.ProjectManagement.Components.ProjectSummary;

public class ProjectSummaryViewComponent: ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        var project = _context.Projects.Include(p=>p.Tasks).FirstOrDefault(p=>p.ProjectId == projectId);
        if (project == null)
        {
            return Content("Project not content");
        }
        return View(project);
    }
    
}