using Data.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger _logger;
        private readonly IStudentRepository _studentRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInUser,
            IRoleRepository roleRepository,
            ILogger<AccountController> logger,
            IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _signInManager = signInUser;
            _roleRepository = roleRepository;
            _logger = logger;
            _studentRepository = studentRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
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
        
        [AllowAnonymous]
        public IActionResult GeneralRegister()
        {
            var registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneralRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName=model.FullName, Gender=model.Gender };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var account = await _userManager.FindByEmailAsync(model.Email);
                    return RedirectToAction("Login");
                }                    

                ModelState.AddModelError("", "Register fail");
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.RegisterViewModel.Email, Email = model.RegisterViewModel.Email, FullName = model.RegisterViewModel.FullName, Gender = model.RegisterViewModel.Gender };
                var student = new Student { Email = model.RegisterViewModel.Email, FullName = model.RegisterViewModel.FullName, Gender = model.RegisterViewModel.Gender };
                var result = await _userManager.CreateAsync(user, model.RegisterViewModel.Password);
                var role_user = new ApplicationUser { UserName = model.Role };

                if (result.Succeeded)
                {
                    var role = new IdentityRole(model.Role);
                    var account = await _userManager.FindByEmailAsync(model.RegisterViewModel.Email);
                    // Add dealerAccount to a dealer role
                    if (!await _userManager.IsInRoleAsync(account, role.Name))
                        await _userManager.AddToRoleAsync(account, role.Name);
                    if (role.Name == "Student")
                    {
                        //save student
                        _studentRepository.AddStudent(student);
                        _studentRepository.Save();
                    }

                    return RedirectToAction("Register");
                }

                ModelState.AddModelError("", "Register fail");
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Login Fail");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }           

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
