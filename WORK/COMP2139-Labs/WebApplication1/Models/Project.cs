using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Project
{
    /// <summary>
    ///  This is the primary key for the project
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// The Name of the project
    /// [Required]: Ensure that the property must be set
    /// </summary>
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    public string? Status { get; set; }
}