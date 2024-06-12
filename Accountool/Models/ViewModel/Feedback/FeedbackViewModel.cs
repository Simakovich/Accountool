using System.ComponentModel.DataAnnotations;

namespace Accountool.Models.ViewModel.Feedback
{
    public class FeedbackViewModel
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
