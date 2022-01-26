using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{

    public class RegisterViewModel
    {

        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
