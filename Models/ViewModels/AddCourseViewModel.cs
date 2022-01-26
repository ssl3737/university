using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class AddCourseViewModel
    {
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public string CourseName { get; set; }
    }
}
