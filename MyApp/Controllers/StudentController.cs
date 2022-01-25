using Data.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.ViewModels;
using System.Collections.Generic;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult StudentUpdate([FromBody] AddStudentViewModel vm)
        {
            var studentId = _studentRepository.GetStudentIdByName(vm.StudentName);
            var student = _studentRepository.GetStudentById(studentId);

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

        public IActionResult Detail(int id)
        {
            var result = _studentRepository.GetStudent(id);
            return View(result);
        }

        public IActionResult Edit(int id)
        {
            var result = _studentRepository.GetStudent(id);
            return View(result);
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

    }
}
