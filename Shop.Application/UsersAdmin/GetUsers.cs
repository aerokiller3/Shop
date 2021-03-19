using System.Collections.Generic;
using System.Linq;
using Shop.Database;

namespace Shop.Application.UsersAdmin
{
    public class GetUsers
    {
        private readonly ApplicationDbContext _ctx;

        public GetUsers(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<UserViewModel> Do()
        {
            var users = _ctx.Users
                .Select(x => new UserViewModel
                {
                    Name = x.UserName,
                    Password = x.PasswordHash
                })
                .OrderBy(x => x.Name)
                .ToList();

            return users;
        }

        public class UserViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}