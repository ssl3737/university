using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    public class StudentCourse
    {        
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
