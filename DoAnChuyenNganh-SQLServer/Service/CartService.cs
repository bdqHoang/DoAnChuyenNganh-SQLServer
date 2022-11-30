using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Service
{
    public class CartService : ICart
    {
        GearShopDataContext _context = new GearShopDataContext();

        public object AddCart(Cart item)
        {
            var data = _context.Carts.Where(s => s.CustomerID == item.CustomerID && s.ProductID == item.ProductID && s.OptionID == item.OptionID && s.ColorID == item.ColorID).FirstOrDefault();
            if(data != null)
            {
                data.Quantity += item.Quantity;
                _context.SubmitChanges();
                return data;
            }
            _context.Carts.InsertOnSubmit(item);
            _context.SubmitChanges();
            return item;
        }

        public object ApplyCoupon(string coupon)
        {
            var data = _context.Discounts.Where(s => s.DiscountID == coupon && s.EndDate >= DateTime.Now && s.NumberOfUse > 0).Select(s=> new { s.DiscountID, s.DiscountPersent }).FirstOrDefault();
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public object CheckOut(List<OrderDetail> order, string Address, Invoice invoice, Order or, Customer customer)
        {
            or.OrderID = CreateOrderID(11);
            foreach(var item in order)
            {
                item.OrderID = or.OrderID;
            }
            invoice.OrderID = or.OrderID;
            invoice.EmployeeID = "ADMIN";
            invoice.InvoiceID = CreateOrderID(11);
            invoice.CreatedAt = DateTime.Now;
            //them vao hoa don
            _context.Orders.InsertOnSubmit(or);
            _context.SubmitChanges();
            _context.Invoices.InsertOnSubmit(invoice);
            _context.SubmitChanges();
            _context.OrderDetails.InsertAllOnSubmit(order);
            _context.SubmitChanges();
            var cart = _context.Carts.Where(s => s.CustomerID == customer.CustomerID).ToList();
            _context.Carts.DeleteAllOnSubmit(cart);
            _context.SubmitChanges();
            string emailTo = customer.Email;
            string subject = "HÓA ĐƠN CỦA BẠN - TEAM-3TL";
            string content = "Mã đơn hàng: " + invoice.InvoiceID + "<br/>" +
                         "Họ tên khách hàng: " + customer.DisplayName + "<br/>" +
                         "Điện thoại: " + customer.Phone + "<br/>" +
                         "Địa chỉ giao hàng:" + Address + "</br>" +
                         "Ngày đặt: " + invoice.CreatedAt.Value.ToString("dd/MM/yyyy hh:mm:ss tt") + "<br/>" +
                         "Sản phẩm đặt mua: <br/>";
            foreach (var item in order)
            {
                content += "Ma san pham: " + item.ProductID + "<br/>" +
                    "Ma mau: " + item.ColorID + "<br/>" +
                    "Lua chon: " + item.OptionID + "<br/>" +
                           "Số lượng: " + item.Quantity + "<br/>" +
                           "Đơn giá: " + item.SellPrice + "<br/>";
            }

            content += "Tổng giá trị đơn hàng là: " + String.Format("{0:0,0}", invoice.TotalPayment) + "VND";
            string body = string.Format(content, "Cảm ơn bạn đã đặt hàng tại shop chúng tôi! </br> Hẹn gặp lại bạn lần sau");
            bool result = EmailService.Send(emailTo, subject, body);
            return new { order, Address, invoice };
        }

        public object DeleteCart(Cart item)
        {
            var data = _context.Carts.Where(s => s.CustomerID == item.CustomerID && s.ProductID == item.ProductID && s.OptionID == item.OptionID && s.ColorID == item.ColorID).FirstOrDefault();
            if (data != null)
            {
                _context.Carts.DeleteOnSubmit(data);
                _context.SubmitChanges();
                return data;
            }
            return null;
        }

        public object GetYourCard(string customerID)
        {
            var data = _context.Carts
                .Where(x => x.CustomerID == customerID).Select(x => new
                {
                    x.ProductID,
                    x.Quantity,
                    x.WareHouse.Product.DisplayName,
                    x.WareHouse.Product.SellPrice,
                    image = Convert.ToBase64String(x.WareHouse.Product.Images.OrderBy(s => s.Ordinal).Select(s => s.Image1).FirstOrDefault().ToArray()),
                    SalePrice = x.WareHouse.Product.SellPrice * (decimal)(100 - x.WareHouse.Product.ProductDiscount.DiscountPercent) / 100,
                    Option = x.WareHouse.Option.DisplayName,
                    Color = x.WareHouse.Color.DisplayName,
                    x.WareHouse.ColorID,
                    x.WareHouse.OptionID
                });
            return new { data = data , total = data.Count() };
        }

        public object UpdateCart(List<Cart> item, string customerID)
        {
            if (item == null)
            throw new NotImplementedException();
            var data = _context.Carts.Where(s => s.CustomerID == customerID);
            _context.Carts.DeleteAllOnSubmit(data);
            _context.SubmitChanges();
            _context.Carts.InsertAllOnSubmit(item);
            _context.SubmitChanges();
            return item;
        }

        // add to cart
        private static Random random = new Random();

        public static string CreateOrderID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}