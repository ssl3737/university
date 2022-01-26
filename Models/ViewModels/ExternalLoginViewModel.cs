using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
