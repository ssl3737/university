using System.Collections.Generic;
using Models.Entities;

namespace Data.Data.Repositories
{
    public interface ICourseRepository
    {
        void AddCourse(Course course);
        IEnumerable<Course> GetAllCourses();
        List<string> GetAllTeachersByCourse(string course);
        Course GetCourses(int id);
        void Edit(Course course);
        void Save();
        int GetCourseIdByTeacherAndCourse(string course, string teacher);
    }
}