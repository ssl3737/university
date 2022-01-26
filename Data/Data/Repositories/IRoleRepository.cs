using Models.Entities;
using System.Collections.Generic;

namespace Data.Data.Repositories
{
    public interface IRoleRepository
    {
        List<string> GetAllRoles();
        List<ApplicationUser> GetAllUsersByRole(string role);
        ApplicationUser GetUserByUserEmail(string user);
        void Update(ApplicationUser applicationUser);
        void Save();
    }
}