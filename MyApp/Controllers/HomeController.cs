using Data.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public HomeController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
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

        [Authorize]
        public IActionResult News()
        {
            return View("News");
        }
    }
}