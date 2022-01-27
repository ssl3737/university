using Microsoft.AspNetCore.Identity;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{
    public class DbSeeder
    {
        private UniversityContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;


        public DbSeeder(UniversityContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabase()
        {
            if (!_context.Courses.Any())
            {
                List<Instructor> instructors = new List<Instructor>()
                {
                    new Instructor() { FullName = "Mike Kim" },
                    new Instructor() { FullName = "Sam Fadi" },
                    new Instructor() { FullName = "Joy Roger" }
                };

                await _context.AddRangeAsync(instructors);
                await _context.SaveChangesAsync();
            }

            var adminAccount = await _userManager.FindByNameAsync("admin@gmail.com");
            var adminRole = new IdentityRole("Admin");
            await _roleManager.CreateAsync(adminRole);
            await _userManager.AddToRoleAsync(adminAccount, adminRole.Name);

            var studentAccount = await _userManager.FindByNameAsync("student@gmail.com");
            var studentRole = new IdentityRole("Student");
            await _roleManager.CreateAsync(studentRole);
            await _userManager.AddToRoleAsync(studentAccount, studentRole.Name);

            var teacherAccount = await _userManager.FindByNameAsync("teacher@gmail.com");
            var teacherRole = new IdentityRole("Teacher");
            await _roleManager.CreateAsync(teacherRole);
            await _userManager.AddToRoleAsync(teacherAccount, teacherRole.Name);
        }
    }
}
