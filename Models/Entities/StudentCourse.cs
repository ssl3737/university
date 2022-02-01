using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class StudentCourse
    {
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
