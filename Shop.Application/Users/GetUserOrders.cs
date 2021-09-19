using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop.Database;
using Shop.Domain.Enums;
using Shop.Domain.Models;

namespace Shop.Application.Users
{
    public class GetUserOrders
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<User> _userManager;

        public GetUserOrders(ApplicationDbContext ctx, UserManager<User> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task<IEnumerable<OrderViewModel>> Do(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            var orders = _ctx.Orders
                .Where(x => x.UserId == user.Id)
                .Select(x => new OrderViewModel
                {
                    OrderRef = x.OrderRef,
                    Status = x.Status,
                    OrderDate = x.OrderDate
                });

            return orders;
        }


        public class OrderViewModel
        {
            public string OrderRef { get; set; }
            public OrderStatus Status { get; set; }
            public DateTime OrderDate { get; set; }
        }
    }
}
