using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
