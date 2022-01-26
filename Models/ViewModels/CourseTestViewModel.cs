using Models.Entities;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class CourseTestViewModel
    {
        public CourseTestViewModel()
        {
            AllCourses = new List<DropDownListViewModel>();
            AllTeachers = new List<DropDownListViewModel>();
        }

        public List<DropDownListViewModel> AllCourses { get; set; }
        public List<DropDownListViewModel> AllTeachers { get; set; }

        public Course Course { get; set; }

        public ICollection<Course> Courses { get; set; }

        public Student Student { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
