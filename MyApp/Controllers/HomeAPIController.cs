using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.Controllers
{

    public class HomeAPIController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IRoleRepository _roleRepository;

        public HomeAPIController(IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IRoleRepository roleRepository,
            IStudentCourseRepository studentCourseRepository,
            UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _roleRepository = roleRepository;
            _studentCourseRepository = studentCourseRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent([FromBody] AddStudentViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Student student = new Student
                    {
                        FullName = vm.StudentName,
                        Email = vm.StudentEmail,
                        Gender = vm.StudentGender
                    };
                    _studentRepository.AddStudent(student);
                    _studentRepository.Save();
                }
                else
                {
                    return Ok(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = "error" });
            }

            return Ok(new { status = "success" });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudentCourse(string id)
        {
            //var userName = User.FindFirst(ClaimTypes.GivenName).Value;
            var userEmail = User.FindFirst(ClaimTypes.Name).Value;
            var studentId = _studentRepository.GetStudentIdByEmail(userEmail);
            var model = _courseRepository.GetCourses(Convert.ToInt32(id));
            var studentModel = _studentRepository.GetStudent(studentId);
            try
            {
                if (ModelState.IsValid)
                {

                    Course course = new Course
                    {
                        TeacherName = model.TeacherName,
                        CourseName = model.CourseName
                    };

                    Student student = new Student
                    {
                        FullName = studentModel.FullName,
                        Email = studentModel.Email,
                        Gender = studentModel.Gender
                    };

                    StudentCourse studentCourse = new StudentCourse
                    {
                        Course = course,
                        CourseId = Convert.ToInt32(id),
                        StudentId = studentId,
                        Student = student
                    };
                    _studentCourseRepository.AddStudentCourse(studentCourse);
                    _studentCourseRepository.Save();
                }
                else
                {
                    return Ok(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = "error" });
            }

            return Ok(new { status = "success" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse([FromBody] AddCourseViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Course course = new Course
                    {
                        TeacherName = vm.TeacherName,
                        CourseName = vm.CourseName,
                    };
                    _courseRepository.AddCourse(course);
                    _courseRepository.Save();
                }
                else
                {
                    return Ok(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = "error" });
            }

            return Ok(new { status = "success" });
        }

        [HttpPost("GetCascadingUsers")]
        [ValidateAntiForgeryToken]
        public IActionResult GetCascadingUsers(string id)
        {
            try
            {
                var users = _roleRepository.GetAllUsersByRole(id);

                return Ok(users);                
            }
            catch (Exception ex)
            {
                return Ok(new { status = "error" });
            }
        }

        [HttpPost("GetCascadingClasses")]
        [ValidateAntiForgeryToken]
        public IActionResult GetCascadingClasses(string id)
        {
            try
            {
                var teachers = _courseRepository.GetAllTeachersByCourse(id);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return Ok(new { status = "error" });
            }
        }

    }
}
