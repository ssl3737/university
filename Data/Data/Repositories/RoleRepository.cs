using Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Data.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UniversityContext _context;
        //private readonly RoleManager<ApplicationUser> _roleManager;

        public RoleRepository(UniversityContext context)
        {
            _context = context;
        }

        public List<string> GetAllRoles()
        {
            var result = _context.Roles.Select(x => x.Name).ToList();

            return result;
        }

        public List<ApplicationUser> GetAllUsersByRole(string role)
        {
            var roleId = _context.Roles.Where(a => a.Name == role).SingleOrDefault().Id;
            List<string> userids = _context.UserRoles
                .Where(a => a.RoleId == roleId)
                .Select(b => b.UserId).Distinct()
                .ToList();
            List<ApplicationUser> users = _context.Users
                .Where(a => userids.Any(c => c == a.Id))            
                .ToList();
            return users;
        }

        public ApplicationUser GetUserByUserEmail(string user)
        {
            ApplicationUser applicationUser = _context.Users
                .Where(a => a.Email == user).SingleOrDefault();
            return applicationUser;
        }
       
        public void Update(ApplicationUser applicationUser)
        {
            _context.Update(applicationUser);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
