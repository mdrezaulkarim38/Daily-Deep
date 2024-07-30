using System.ComponentModel.DataAnnotations;

namespace Daily_Deep.Models;

public class TransactionData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime TransactionDate { get; set; }

    [Required]
    public string? CategoryCode { get; set; }

    [Required]
    public string? TransactionType { get; set; }

    [Required]
    [StringLength(255)]
    public string? Description { get; set; }

    [Required]
    public decimal Amount { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
