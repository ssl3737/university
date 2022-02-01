using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(60)]
        public string FullName { get; set; }

        [Required]
        [StringLength(60)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string Gender { get; set; }
    }
}
