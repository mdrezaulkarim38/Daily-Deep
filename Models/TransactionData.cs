using System.ComponentModel.DataAnnotations;

namespace Daily_Deep.Models;
public class TransactionData
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? TransactionDetails { get; set; }
    [Required]
    public string? TransactionDate { get; set; }
    [Required]
    public string? TransactionType { get; set; }
    [Required]
    public string? TransactionAmount { get; set; }

    [Display(Name = "User Id")]
    public int? UserId { get; set; }
}
