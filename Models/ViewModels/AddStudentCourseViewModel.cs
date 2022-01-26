namespace Models.ViewModels
{
    public class AddStudentCourseViewModel
    {
        public int CourseId { get; set; }
        public string TeacherName { get; set; }
        public string CourseName { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
