using Accountool.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Accountool.Models.Services.Identity
{
    public interface IIdentityService
    {
        Task<List<string>> GetAdminEmailsAsync();
        Task SetSingleRoleByEmailAsync(string userEmail, Role role);
        Task SetDefaultRoles();
    }
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<string>> GetAdminEmailsAsync()
        {
            var adminEmails = new List<string>();
            var adminRole = Role.Admin.ToString();

            var isAdminRoleExists = await _roleManager.FindByNameAsync(adminRole);
            if (isAdminRoleExists != null)
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync(adminRole);
                if (adminUsers != null && adminUsers.Any())
                {
                    adminEmails.AddRange(adminUsers?.Select(x => x.Email));
                }
            }

            return adminEmails;
        }

        public async Task SetSingleRoleByEmailAsync(string userEmail, Role role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new Exception("The user not found");
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, role.ToString());
            }
        }

        public async Task SetDefaultRoles()
        {
            foreach (var role in Enum.GetNames(typeof(Role)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
