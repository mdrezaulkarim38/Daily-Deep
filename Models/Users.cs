using System.ComponentModel.DataAnnotations;
namespace Daily_Deep.Models;

public class Users
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Full Name")]
    [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
    public string? FullName { get; set; }

    [Required]
    [Display(Name = "Username")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public string? Username { get; set; }

    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? Password { get; set; }
}