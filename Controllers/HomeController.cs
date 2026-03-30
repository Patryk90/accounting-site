using AccountingPortfolio.Models;
using AccountingPortfolio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountingPortfolio.Controllers;

public class HomeController : Controller
{
    private readonly EmailQueueService _emailQueue;
    private readonly ILogger<HomeController> _logger;

    public HomeController(EmailQueueService emailQueue, ILogger<HomeController> logger)
    {
        _emailQueue = emailQueue;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact([FromForm] ContactFormModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please fill all fields correctly.";
            return RedirectToAction("Index");
        }

        var emailBody = $@"
            <p><strong>Name:</strong> {model.Name}</p>
            <p><strong>Email:</strong> {model.Email}</p>
            <p><strong>Message:</strong></p>
            <p>{model.Message}</p>";

        await _emailQueue.EnqueueEmailAsync(model.Email, "New Contact Form Submission", emailBody);

        TempData["SuccessMessage"] = "Your message has been queued successfully!";
        return RedirectToAction("Index");
    }
}