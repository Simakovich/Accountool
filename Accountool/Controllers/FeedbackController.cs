using Accountool.Models.Services.EmailService;
using Accountool.Models.Services.EmailService.Model;
using Accountool.Models.Services.Identity;
using Accountool.Models.ViewModel.Feedback;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public FeedbackController(
            IEmailService emailService,
            IIdentityService identityService)
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
        public async Task<IActionResult> SendFeedback(FeedbackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var adminEmails = await _identityService.GetAdminEmailsAsync();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<FeedbackViewModel, FeedbackModel>());
                var mapper = config.CreateMapper();
                var model = mapper.Map<FeedbackModel>(viewModel);
                if (adminEmails.Any())
                {
                    var emailMessage = await _emailService.PrepareSupportEmailMessage(adminEmails, "Новое сообщение обратной связи", model);
                    await _emailService.SendEmailAsync(emailMessage);
                }

                return RedirectToAction("FeedbackSent");
            }

            return View("Index", viewModel);
        }

        public ActionResult FeedbackSent()
        {
            return View();
        }
    }
}
