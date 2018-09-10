using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            AllRoles = new List<DropDownListViewModel>();
        }

        public List<DropDownListViewModel> AllRoles { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; }

        public IEnumerable<RegisterViewModel> RegisterViewModels { get; set; }

        public string Role { get; set; }
    }
}
