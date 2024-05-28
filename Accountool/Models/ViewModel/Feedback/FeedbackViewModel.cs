using Microsoft.Build.Framework;

namespace Accountool.Models.ViewModel.Feedback
{
    public class FeedbackViewModel
    {
        [Required]
        public string Message { get; set; }
    }
}
