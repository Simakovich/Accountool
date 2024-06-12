using System.ComponentModel.DataAnnotations;

namespace Accountool.Models.Services.EmailService.Model
{
    public class FeedbackModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}