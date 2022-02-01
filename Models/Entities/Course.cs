using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        public Instructor Instructor { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
