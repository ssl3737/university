using Data.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.ViewModels;
using System.Collections.Generic;

namespace University.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IRoleRepository _roleRepository; 
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseRepository courseRepository,
            IStudentCourseRepository studentCourseRepository,
            IRoleRepository roleRepository,
            UserManager<ApplicationUser> userManager)
        {
            _courseRepository = courseRepository;
            _studentCourseRepository = studentCourseRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult ViewCourse()
        {
            var allCourses = _courseRepository.GetAllCourses();
            var courses = new List<Course>();

            foreach (var c in allCourses)
            {
                courses.Add(new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName
                });
            }

            var viewModel = new CourseTestViewModel
            {
                Courses = courses
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public PartialViewResult ViewCourse(CourseTestViewModel viewModel)
        {
            return PartialView("_CourseTable", viewModel);
        }

        [Authorize]
        public IActionResult EnrollCourse()
        {
            var currentUserName = _userManager.GetUserAsync(HttpContext.User).Result.FullName;
            var allCourses = _courseRepository.GetAllCourses();
            var studentCourses = new List<AddStudentCourseViewModel>();

            foreach (var c in allCourses)
            {
                studentCourses.Add(new AddStudentCourseViewModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName,
                    IsEnrolled = _studentCourseRepository.FindStudentCourse(currentUserName, c.CourseId) == null ? false : true

                });
            }

            var viewModel = new StudentCourseViewModel
            {
                AddStudentCourseViewModels = studentCourses
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult CourseUpdate([FromBody] AddCourseViewModel vm)
        {
            var id = _courseRepository.GetCourseIdByTeacherAndCourse(vm.CourseName, vm.TeacherName);
            var course = _courseRepository.GetCourses(id);
            var allCourses = _courseRepository.GetAllCourses();
            var courses = new List<Course>();
            var courseModel = new Course
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                TeacherName = course.TeacherName
            };

            foreach (var c in allCourses)
            {
                courses.Add(new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName
                });
            }

            var viewModel = new CourseTestViewModel
            {
                Courses = courses,
                Course = courseModel
            };

            return PartialView("_SelectedCourse", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CourseEdit(CourseTestViewModel viewModel)
        {
            var model = new Course
            {
                CourseName = viewModel.Course.CourseName,
                TeacherName = viewModel.Course.TeacherName
            };
            //save
            _courseRepository.Edit(model);
            _courseRepository.Save();
            return RedirectToAction("CreateCourse");
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult CreateCourse()
        {
            var courses = _courseRepository.GetAllCourses();
            var users = _roleRepository.GetAllUsersByRole("Teacher");
            var CoursesModel = new List<Course>();
            foreach (var c in courses)
            {
                CoursesModel.Add(new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName
                });
            }
            var viewModel = new CourseTestViewModel()
            {
                Courses = CoursesModel
            };

            foreach (var user in users)
            {
                viewModel.AllTeachers.Add(new DropDownListViewModel
                {
                    Id = user.FullName,
                    Text = user.FullName
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public PartialViewResult Createcourse(CourseTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {

                };
                //save
                _courseRepository.AddCourse(model.Course);
                _courseRepository.Save();
            }
            else
            {
                //error
            }
            var courses = _courseRepository.GetAllCourses();
            var CoursesModel = new List<Course>();
            foreach (var c in courses)
            {
                CoursesModel.Add(new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName
                });
            }

            var viewModel = new CourseTestViewModel()
            {
                Courses = CoursesModel
            };

            return PartialView("_CourseTable", viewModel);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult ManageCourse()
        {
            var courses = _courseRepository.GetAllCourses();
            var users = _roleRepository.GetAllUsersByRole("Teacher");

            var CoursesModel = new List<Course>();
            foreach (var c in courses)
            {
                CoursesModel.Add(new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName
                });
            }

            var viewModel = new CourseTestViewModel()
            {
                Courses = CoursesModel
            };

            foreach (var course in courses)
            {
                viewModel.AllCourses.Add(new DropDownListViewModel
                {
                    Id = course.CourseName,
                    Text = course.CourseName
                });
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                //save
                _courseRepository.Edit(course);
                _courseRepository.Save();
                return RedirectToAction("Student", "Student");
            }
            return View(course);
        }

    }
}
