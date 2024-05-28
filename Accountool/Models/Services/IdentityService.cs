using Accountool.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Accountool.Models.Services
{
    public interface IIdentityService
    {
    }
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(
            UserManager<AspNetUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<string>> GetAdminEmailsAsync()
        {
            var adminEmails = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync("admin");
            if (adminRole != null)
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync("admin");
                adminEmails.AddRange(adminUsers.Select(x => x.Email));
            }

            return adminEmails;
        }
    }
}
