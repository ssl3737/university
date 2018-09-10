using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Course
    {        
        public int CourseId { get; set; }

        [Required]
        public string TeacherName { get; set; }

        [Required]
        public string CourseName { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
