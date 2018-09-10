using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            AllStudents = new List<DropDownListViewModel>();
        }

        public List<DropDownListViewModel> AllStudents { get; set; }

        public Student Student { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
