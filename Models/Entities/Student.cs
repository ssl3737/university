using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
        
    }
}
