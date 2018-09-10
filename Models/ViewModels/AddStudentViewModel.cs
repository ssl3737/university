using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AddStudentViewModel
    {

        [Required]
        [MaxLength(50)]
        public string StudentName { get; set; }

        [Required]
        public string StudentEmail { get; set; }

        [Required]
        public string StudentGender { get; set; }
        
    }
}
