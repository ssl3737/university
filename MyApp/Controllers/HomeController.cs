using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Data.Repositories;
using Models.Entities;
using Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            IRoleRepository roleRepository,
            IStudentCourseRepository studentCourseRepository,
            UserManager<ApplicationUser> userManager)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _studentCourseRepository = studentCourseRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
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
        
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {

            var roles = _roleRepository.GetAllRoles();
            var roleViewModel = new RoleViewModel();

            foreach (var role in roles)
            {
                roleViewModel.AllRoles.Add(new DropDownListViewModel
                {
                    Id = role,
                    Text = role
                });
            }
            var adminViewModel = new AdminViewModel
            {
                AllRoles = roleViewModel.AllRoles
            };

            return View(adminViewModel);
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
        [Authorize(Roles = "Admin")]
        public IActionResult Admin(string userEmail)
        {
            var applicationUser = _roleRepository.GetUserByUserEmail(userEmail);
            var viewModel = new ApplicationUserViewModel
            {
                UserEmail = userEmail,
                FullName = applicationUser.FullName,
                Gender = applicationUser.Gender
            };
            return PartialView("_SelectedUser", viewModel);
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult StudentEdit(StudentViewModel viewModel)
        {
            var model = new Student
            {
                FullName = viewModel.Student.FullName,
                Gender = viewModel.Student.Gender,
                Email = viewModel.Student.Email
            };
            //save
            _studentRepository.Edit(model);
            _studentRepository.Save();
            return RedirectToAction("Student");
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Update(ApplicationUserViewModel user)
        {
            var model = _roleRepository.GetUserByUserEmail(user.UserEmail);
            model.FullName = user.FullName;
            model.Gender = user.Gender;

            //save
            _roleRepository.Update(model);
            _roleRepository.Save();
            return RedirectToAction("Admin");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Student()
        {
            var students = _studentRepository.GetAllStudents();

            var StudentsModel = new List<Student>();
            foreach (var s in students)
            {
                StudentsModel.Add(new Student
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName,
                    Gender = s.Gender,
                    Email = s.Email
                });
            }

            var viewModel = new StudentViewModel()
            {
                Students = StudentsModel
            };
            foreach (var stdent in students)
            {
                viewModel.AllStudents.Add(new DropDownListViewModel
                {
                    Id = stdent.FullName,
                    Text = stdent.FullName
                });
            }
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult StudentUpdate([FromBody] AddStudentViewModel vm)
        {
            var studentId = _studentRepository.GetStudentIdByName(vm.StudentName);
            var student = _studentRepository.GetStudentById(studentId);
            
            var students = new List<Student>();
            var studentModel = new Student
            {
                StudentId = student.StudentById[0].StudentId,
                Email = student.StudentById[0].Email,
                FullName = student.StudentById[0].FullName,
                Gender = student.StudentById[0].Gender
            };

            var viewModel = new StudentViewModel
            {
                Student = studentModel
            };
            return PartialView("_SelectedStudent", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var result = _studentRepository.GetStudent(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public PartialViewResult Student(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //save
                _studentRepository.AddStudent(model.Student);
                _studentRepository.Save();           
            }
            else
            {
                //error
            }
            var students = _studentRepository.GetAllStudents();
            var viewModel = new StudentCourseViewModel()
            {              
                Students = students
            };
            
            return PartialView("_SelectedStudent", viewModel);
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

        public IActionResult Detail(int id)
        {
            var result = _studentRepository.GetStudent(id);
            return View(result);
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
                return RedirectToAction("Student");
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                //save
                _studentRepository.Edit(student);
                _studentRepository.Save();
                return RedirectToAction("Student");

            }
            return View(student);
        }


        public IActionResult Delete(int id)
        {
            var result = _studentRepository.GetStudent(id);

            if (result != null)
            {
                _studentRepository.Delete(result);
                _studentRepository.Save();
            }
            return RedirectToAction("Student");
        }

        [Authorize]
        public IActionResult News()
        {
            return View("News");
        }
    }
}