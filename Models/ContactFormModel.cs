using System.ComponentModel.DataAnnotations;

namespace AccountingPortfolio.Models;

public class ContactFormModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$",
        ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 5,
        ErrorMessage = "Message must be between 5 and 2000 characters.")]
    public string Message { get; set; }
}