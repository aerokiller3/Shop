using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Database;

namespace Shop.Application.UsersAdmin
{
    public class DeleteUser
    {
        //private ApplicationDbContext _ctx;
        //private UserManager<IdentityUser> _userManager;

        //public DeleteUser(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
        //{
        //    _ctx = ctx;
        //    _userManager = userManager;
        //}

        //public async Task<bool> Do(string id)
        //{
        //    var user = _ctx.Users.FirstOrDefault(x => x.Email == id);
        //    _ctx.Users.Remove(user);
        //    await _ctx.SaveChangesAsync();

        //    var managerrUser = new IdentityUser
        //    {
        //        UserName = user.UserName
        //    };

        //    await _userManager.DeleteAsync(managerrUser);

        //    Claim managerClaim = _userManager.GetClaimsAsync(managerrUser);

        //    await _userManager.RemoveClaimAsync(managerrUser, managerClaim);

        //    return true;
        //}
    }
}
