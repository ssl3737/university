using System.Collections.Generic;

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
