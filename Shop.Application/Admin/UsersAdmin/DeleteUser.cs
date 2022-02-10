using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Domain.Models;

namespace Shop.Application.Admin.UsersAdmin
{
    public class DeleteUser
    {
        private readonly UserManager<User> _userManager;

        public DeleteUser(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Do(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}