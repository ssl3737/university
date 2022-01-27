using Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class AddCourseViewModel
    {
        [Required]
        public string InstructorName { get; set; }
        [Required]
        public string CourseName { get; set; }
    }
}
