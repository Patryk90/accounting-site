using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountingPortfolio.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Success { get; set; }

    public class InputModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        Success = true;
        Input = new InputModel();
        return Page();
    }
}