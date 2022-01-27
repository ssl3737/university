using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Data.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityContext _context;

        public CourseRepository(UniversityContext context)
        {
            _context = context;
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var result = _context.Courses
                .Include(i => i.Instructor)
                .ToList();
            return result;
        }

        public int GetCourseIdByTeacherAndCourse(string course, string instructor)
        {
            var id = _context.Courses.Where(a => a.CourseName == course && a.Instructor.FullName == instructor).SingleOrDefault().CourseId;
            return id;
        }

        public List<string> GetAllTeachersByCourse(string course)
        {
            var courseId = _context.Courses.Where(a => a.CourseName == course).SingleOrDefault().CourseId;
            List<string> teachers = _context.Courses
                .Where(a => a.CourseId == courseId)
                .Select(b => b.Instructor.FullName).Distinct()
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
