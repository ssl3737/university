using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
