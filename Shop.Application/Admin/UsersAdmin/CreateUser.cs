using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Domain.Models;

namespace Shop.Application.Admin.UsersAdmin
{
    public class CreateUser
    {
        private readonly UserManager<User> _userManager;

        public CreateUser(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public class Request
        {
            public string UserName { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var managerUser = new User
            {
                UserName = request.UserName,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(managerUser, "password");

            var managerClaim = new Claim("Role", "Manager");

            await _userManager.AddClaimAsync(managerUser, managerClaim);

            return true;
        }
    }
}
