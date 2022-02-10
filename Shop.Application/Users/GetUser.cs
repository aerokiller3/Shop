using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Domain.Models;

namespace Shop.Application.Users
{
    public class GetUser
    {
        private readonly UserManager<User> _userManager;

        public GetUser(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserViewModel> Do(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            return new UserViewModel
            {
                SurName = user.SurName,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Orders = user.Orders
            };
        }

        public class UserViewModel
        {
            public string SurName { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public List<Order> Orders { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
