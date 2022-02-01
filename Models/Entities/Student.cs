using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(60)]
        public string FullName { get; set; }

        [StringLength(60)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string Email { get; set; }

        [Required]
        [StringLength(60)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string Gender { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
        
    }
}
