using Accountool.Models.Services;
using Accountool.Models.ViewModel.Feedback;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IdentityService _identityService;

        public FeedbackController(
            IEmailService emailService,
            IdentityService identityService)
        {
            _emailService = emailService;
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendFeedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminEmails = await _identityService.GetAdminEmailsAsync();

                if (adminEmails.Any())
                {
                    _emailService.SendEmailAsync(adminEmails, "Новое сообщение обратной связи", model.Message);
                }

                return RedirectToAction("FeedbackSent"); // представление для успешной отправки сообщения
            }

            return View("Index", model);
        }
    }
}
