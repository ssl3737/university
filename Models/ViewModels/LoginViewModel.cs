using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
