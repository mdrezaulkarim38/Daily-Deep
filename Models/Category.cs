using System.ComponentModel.DataAnnotations;

namespace Daily_Deep.Models;
public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "Category Name Cannot be Longer then 50 characters.")]
    public string? CategoryName { get; set; }
    
    [Required]
    [Display(Name = "Category Code")]
    public int? CategoryCode { get; set; }
    
    [Display(Name = "User Id")]
    public int? UserId { get; set; }
}