using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Areas.ProjectManagement.Models;


public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(100,ErrorMessage="Project Name cannot be more than 100 characters.")]
    public required string Title{ get; set; }
    
    [Required]
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500,ErrorMessage="Project Name cannot be more than 100 characters.")]
    public required string Description{ get; set; }
    
    
    [Display(Name = "Parent Project ID")]
    public int ProjectId { get; set; }
    public Project?  Project { get; set; }
    
}