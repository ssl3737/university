using System;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public DateTime StudentEnrollmentDate { get; set; }
    }
}
