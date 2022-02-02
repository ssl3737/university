using Data.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Entities;
using Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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
                    Email = s.Email,
                    EnrollmentDate = s.EnrollmentDate
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
        public async Task<PartialViewResult> Student(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //save
                await _studentRepository.AddStudentAsync(model.Student);
                await _studentRepository.SaveAsync();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> StudentUpdate([FromBody] AddStudentViewModel vm)
        {
            var student = await _studentRepository.GetStudentByName(vm.StudentName);

            var studentModel = new Student
            {
                StudentId = student.StudentId,
                Email = student.Email,
                FullName = student.FullName,
                Gender = student.Gender,
                EnrollmentDate = student.EnrollmentDate
            };

            var viewModel = new StudentViewModel
            {
                Student = studentModel
            };

            return PartialView("_SelectedStudent", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(StudentViewModel viewModel)
        {
            var model = new Student
            {
                FullName = viewModel.Student.FullName,
                Gender = viewModel.Student.Gender,
                Email = viewModel.Student.Email,
                EnrollmentDate = viewModel.Student.EnrollmentDate
            };
            //save
            _studentRepository.Edit(model);
            await _studentRepository.SaveAsync();
            return RedirectToAction("Student");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentRepository.GetStudentAsync(id);

            if (student != null)
                return View(student);
            else
                return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _studentRepository.GetStudentAsync(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                //save
                _studentRepository.Edit(student);
                await _studentRepository.SaveAsync();
                return RedirectToAction("Student");

            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentRepository.GetStudentAsync(id);

            if (result != null)
            {
                _studentRepository.Delete(result);
                await _studentRepository.SaveAsync();
            }
            return RedirectToAction("Student");
        }

        // GET: /Student/Create
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            var student = new Student();
            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _studentRepository.AddStudentAsync(student);
                    await _studentRepository.SaveAsync();
                    return RedirectToAction("Student");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. try again, and if the problem persists see your system administrator.");
            }

            return View(student);
        }

    }
}
