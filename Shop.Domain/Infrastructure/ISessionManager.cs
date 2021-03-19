using System.Collections.Generic;
using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure
{
    public interface ISessionManager
    {
        string GetId();
        void AddProduct(CartProduct cartProduct);
        void RemoveProduct(int stockId, int qty);
        IEnumerable<CartProduct> GetCart();

        void AddCustomerInformation(CustomerInformation customerInformation);
        CustomerInformation GetCustomerInformation();
    }
}
