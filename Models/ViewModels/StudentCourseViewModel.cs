using Models.Entities;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class StudentCourseViewModel
    {
        public Student Student { get; set; }

        public Course Course { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Student> Students { get; set; }

        public AddStudentCourseViewModel AddStudentCourseViewModel { get; set; }
        public IEnumerable<AddStudentCourseViewModel> AddStudentCourseViewModels { get; set; }


    }
}
