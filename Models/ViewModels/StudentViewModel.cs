using Models.Entities;
using System.Collections.Generic;

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
