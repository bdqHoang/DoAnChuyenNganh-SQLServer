using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh_SQLServer.Interface
{
    public interface ICart
    {
        object GetYourCard(string customerID);
        object AddCart(Cart item);
        object DeleteCart(Cart item);
        object UpdateCart(List<Cart> item, string customerID);

        object ApplyCoupon(string coupon);

        object CheckOut(List<OrderDetail> order, string Address, Invoice invoice, Order or, Customer customer);
    }
}
