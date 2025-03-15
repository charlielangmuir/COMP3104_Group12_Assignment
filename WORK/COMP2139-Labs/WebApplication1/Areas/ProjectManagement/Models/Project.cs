using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Areas.ProjectManagement.Models;

public class Project{
    ///<summary>
    ///This is the primary key for the project
    ///</summary>
    public int ProjectId{get;set;}



    ///<summary>
    ///The Name of the Project
    ///[Required]:Ensures this property must be set
    ///</summary>
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100,ErrorMessage="Project Name cannot be more than 100 characters.")]
    public required string Name{get; set;}
    [Display(Name = "Description")]
    [StringLength(500,ErrorMessage="Project Name cannot be more than 100 characters.")]
    public string? Description{get; set;}

    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
     public DateTime EndDate{get; set;}= DateTime.UtcNow.AddDays(7);
        [Display(Name = "Project Status")]
     public string? Status{get; set;}

     public List<ProjectTask>? Tasks { get; set; } = new();

}