using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Daily_Deep.Models;


namespace Daily_Deep.ViewModel;

public class TransactionViewModel
{
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

    public SelectList CategorySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
