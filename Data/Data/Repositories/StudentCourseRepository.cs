using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data.Repositories
{
    public class StudentCourseRepository : IStudentCourseRepository
    { 
        private readonly UniversityContext _context;

        public StudentCourseRepository(UniversityContext context)
        {
            _context = context;

        }

        public void AddStudentCourse(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Edit(StudentCourse studentCourse)
        {
            _context.Update(studentCourse);
        }

        public StudentCourse FindStudentCourse(string userName, int courseId)
        {
            var studentId = _context.Students.Where(a => a.FullName == userName).SingleOrDefault().StudentId;
            StudentCourse studentCourse = _context.StudentCourses
                .Where(a => a.StudentId == studentId && a.CourseId == courseId).SingleOrDefault();
            return studentCourse;
        }
    }
}
