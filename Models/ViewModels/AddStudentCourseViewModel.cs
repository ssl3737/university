using Models.Entities;

namespace Models.ViewModels
{
    public class AddStudentCourseViewModel
    {
        public int CourseId { get; set; }
        public Instructor Instructor { get; set; }
        public string CourseName { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
