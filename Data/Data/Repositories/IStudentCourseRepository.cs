using Models.Entities;

namespace Data.Data.Repositories
{
    public interface IStudentCourseRepository
    {
        void AddStudentCourse(StudentCourse studentCourse);
        void Edit(StudentCourse studentCourse);
        void Save();
        StudentCourse FindStudentCourse(string userName, int courseId);
    }
}