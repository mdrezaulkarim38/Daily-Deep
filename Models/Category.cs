using System.ComponentModel.DataAnnotations;

namespace Daily_Deep.Models;
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Category Name")]
    [StringLength(50, ErrorMessage = "Category Name cannot be longer than 50 characters.")]
    public string? CategoryName { get; set; }

    [Required]
    [Display(Name = "Category Code")]
    [StringLength(10, ErrorMessage = "Category Code cannot be longer than 10 characters.")]
    public string? CategoryCode { get; set; }

    [Display(Name = "User Id")]
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
