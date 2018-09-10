using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly MyAppContext _context;

        public CourseRepository(MyAppContext context)
        {
            _context = context;
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var result = _context.Courses.ToList();
            return result;
        }

        public int GetCourseIdByTeacherAndCourse(string course, string teacher)
        {
            var id = _context.Courses.Where(a => a.CourseName == course && a.TeacherName == teacher).SingleOrDefault().CourseId;
            return id;
        }

        public List<string> GetAllTeachersByCourse(string course)
        {
            var courseId = _context.Courses.Where(a => a.CourseName == course).SingleOrDefault().CourseId;
            List<string> teachers = _context.Courses
                .Where(a => a.CourseId == courseId)
                .Select(b => b.TeacherName).Distinct()
                .ToList();
            return teachers;
        }
        public Course GetCourses(int id)
        {
            var result = _context.Courses.Find(id);
            return result;
        }

        public void Edit(Course course)
        {
            _context.Update(course);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
