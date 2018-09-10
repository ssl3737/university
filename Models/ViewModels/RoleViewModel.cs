using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            AllRoles = new List<DropDownListViewModel>();
        }

        public List<DropDownListViewModel> AllRoles { get; set; }
    }
}
