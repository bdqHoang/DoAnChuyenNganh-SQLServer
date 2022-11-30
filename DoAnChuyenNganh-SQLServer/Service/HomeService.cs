using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web.Helpers;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Models;
using DoAnChuyenNganh_SQLServer.Service;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DoAnChuyenNganh.Service
{
    public class HomeService : IHome
    {
        private GearShopDataContext _context = new GearShopDataContext();
        /// <summary>
        /// show supplier have product
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ListBrand(String id)
        {
            var data = _context.Suppliers.Where(s => s.Status != false && s.Products.Where(m=>m.CategorizeID == id && m.Status != false).Count() != 0)
                .Include(s => s.Products).Select(s=> new { s.SupplierID, s.DisplayName}).ToList();
            return data;
        }

        /// <summary>
        /// code show categorize
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ListCategorize()
        {
            var data = _context.Categorizes.Where(s=>s.Status != false).Select(s=> new { s.CategorizeID,s.DisplayName}).ToList();
            return data;
        }
        /// <summary>
        /// show categori best seller
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ListTopCategories()
        {
            //var ProductSale = _context.OrderDetails.GroupBy(s => s.ProductID).Select(s => new { 
            //    s.Key,
            //    quantity = s.Sum(m => m.Quantity)
            //});
            var data = _context.Categorizes.Include(s=>s.Products)
                .Where(s => s.Status != false && s.Products.Where(m=>m.Status!=false).Count()!=0)
                .Select(s=> new { s.CategorizeID, s.DisplayName, 
                    image = CheckExitImage(s.Products.FirstOrDefault(m => m.Status != false).ProductID)
                }).Take(6);
            return data;
        }
        /// <summary>
        /// show list product saler
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ListSaleProduct()
        {
            var data = _context.Products.Include(s => s.ProductDiscount).Include(s=>s.Categorize).Include(s=>s.Images)
                .Where(s => s.ProductDiscount.EndDate >= DateTime.Now && s.Status != false && s.Categorize.Status!=false && s.ProductDiscountID != null)
                .Select(s => new { s.ProductID,s.DisplayName,s.ProductDiscount.DiscountPercent, oldPrice = s.SellPrice,
                    salePrice = s.SellPrice * (decimal)(100-s.ProductDiscount.DiscountPercent)/100,EndDate = s.ProductDiscount.EndDate.Value.ToShortDateString(),
                    Categorize = s.Categorize.DisplayName,
                    image = CheckExitImage(s.ProductID)});
            return data;
        }
        /// <summary>
        /// show product best saler
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ProductBestSaler()
        {
            var data = _context.Products.Include(s => s.WareHouses).ThenInclude(s => s.OrderDetails).Include(s=>s.Images).Include(s=>s.ProductDiscount).Include(s=>s.Categorize)
                .GroupBy(s => s.ProductID)
                .Select(s => new { s.Key,DisplayName = s.Select(m=>m.DisplayName).FirstOrDefault(),SellPrice = s.Select(m=>m.SellPrice).FirstOrDefault(),
                    SalePrice = s.Select(m=>m.SellPrice).FirstOrDefault() * (decimal)(100-s.Where(m=>m.ProductDiscountID != null && m.ProductDiscount.EndDate >= DateTime.Now).Select(m=>m.ProductDiscount.DiscountPercent).FirstOrDefault())/100,
                    image = CheckExitImage(s.Key),
                    Categorize = s.Select(m => m.Categorize.DisplayName),
                    quantity = s.Sum(v => v.WareHouses.Sum(n => n.OrderDetails.Sum(q => q.Quantity))) });
            var result = data.OrderByDescending(s => s.quantity).First();
            return result;
        }
        /// <summary>
        /// show trendding item
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object TrenddingItem()
        {
            var data = _context.Products.Include(s=>s.Images).Include(s=>s.ProductDiscount).Include(s=>s.Categorize)
                .Where(s => s.Status != false).OrderBy(s => s.NumberOfLike)
                .Select(s => new { s.ProductID, s.DisplayName, s.SellPrice, s.ProductDiscount.DiscountPercent, 
                    Categorize = s.Categorize.DisplayName,
                    SalePrice=s.SellPrice *(decimal)(100-s.ProductDiscount.DiscountPercent)/100, image = CheckExitImage(s.ProductID)});
            return (data);
        }
        /// <summary>
        /// show new product
        /// </summary>
        /// <returns></returns>
        public object NewArrival()
        {
            var data = _context.Products.Include(s => s.Images).Include(s => s.Categorize).Include(s => s.ProductDiscount).Include(s => s.Supplier)
                .Where(s => s.Status != false).OrderBy(s=>s.CreatedAt)
                .Select(s => new
                {
                    s.ProductID,
                    s.DisplayName,
                    Categorize = s.Categorize.DisplayName,
                    s.SellPrice,
                    s.ProductDiscount.DiscountPercent,
                    SalePrice = s.SellPrice * (decimal)(100 - s.ProductDiscount.DiscountPercent) / 100,
                    image = CheckExitImage(s.ProductID)
                });
            return data;
        }
        /// <summary>
        /// show detail product
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object DetailProduct(string id)
        {
            var data = _context.Products
                .Where(s => s.Status != false && s.ProductID == id)
                .Select(s => new { s.ProductID, s.DisplayName, Categorize = s.Categorize.DisplayName, s.CategorizeID,
                    Supplier = s.Supplier.DisplayName, s.ProductDiscount.DiscountPercent, s.SellPrice,
                    SalePrice = s.SellPrice * (decimal)(100 - s.ProductDiscount.DiscountPercent) / 100, s.Rating, Comment = s.FeedBacks.Select(v => new {v.Customer.DisplayName, v.Rating, v.Content}),
                    Color = s.WareHouses.GroupBy(v=>v.ColorID).Select(v => v.Select(m=>new { m.Color.DisplayName, m.Color.ColorID }).Distinct()),
                    Option = s.WareHouses.GroupBy(v => v.OptionID).Select(v => v.Select(m =>new { m.Option.DisplayName, m.Option.OptionID }).Distinct()),
                    s.Description,
                    image = s.Images.Where(v=>v.Image1 != null).OrderByDescending(v=>v.Ordinal).Select(v=>Convert.ToBase64String(v.Image1.ToArray())).Take(4),
                    Quantity = s.WareHouses.GroupBy(v=>v.ProductID).Select(v=>v.Sum(m=>m.quantity)).FirstOrDefault(),
                }).FirstOrDefault();
            return data;
                
        }
        /// <summary>
        /// getdate product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product GetProduct(string id)
        {
            return _context.Products.FirstOrDefault(s => s.ProductID == id);
        }
        /// <summary>
        /// get quantity product in warehouse
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="colorID"></param>
        /// <param name="optioID"></param>
        /// <returns></returns>
        public int Quantity(string productid, string colorID = null, string optioID = null)
        {
            int quantity=0;
            if (String.IsNullOrEmpty(colorID))
            {
                return quantity =(int)_context.WareHouses.Where(s => s.ProductID == productid && s.OptionID == optioID).GroupBy(s => s.ProductID).Sum(s => s.Select(v => v.quantity).FirstOrDefault());
            }
            if (String.IsNullOrEmpty(optioID))
            {
                return quantity = (int)_context.WareHouses.Where(s => s.ProductID == productid && s.ColorID == colorID).GroupBy(s => s.ProductID).Sum(s => s.Select(v => v.quantity).FirstOrDefault());
            }
            var data = _context.WareHouses.Where(s => s.ProductID == productid && s.ColorID == colorID && s.OptionID == optioID).FirstOrDefault();
            if(data == null)
            {
                return -1;
            }
            return (int)data.quantity;
        }

        /// <summary>
        /// recommend product
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object RecommentProduct(string customerID)
        {
           if(customerID == null)
            {
                var data = _context.Products.Where(s => s.Status != false).OrderBy(s => s.CreatedAt)
                    .Select(s => new
                    {
                        s.ProductID,
                        s.DisplayName,
                        s.SellPrice,
                        s.ProductDiscount.DiscountPercent,
                        Categorize = s.Categorize.DisplayName,
                        SalePrice = s.SellPrice * (decimal)(100 - s.ProductDiscount.DiscountPercent) / 100,
                        image = CheckExitImage(s.ProductID)
                    });
                return data;
            }
            else
            {
                INBCF guess = new NBCFService();
                var recomment = guess.RecommentForYou(customerID);
                List<object> dataRecomment = new List<object>();
                foreach(var item in recomment)
                {
                    dataRecomment.Add(GetInforProduct(item));
                }
                return dataRecomment;
            }
        }
        /// <summary>
        /// show supplier
        /// </summary>
        /// <returns></returns>
        public object ListSuppliers()
        {
            var data = _context.Suppliers.Where(s => s.Status != false).Select(s => new {s.SupplierID,s.DisplayName}).Take(4).ToList();
            return data;
        }






        //------------------------------function-------------------------------------------------
        //check exit image
        private string CheckExitImage(string product)
        {
            var data = _context.Images.Where(n => n.ProductID == product);
            
            if (data.Where(s=>s.Image1 != null).OrderBy(s=>s.Ordinal).Count() == 0)
            {
                return null;
            }
            var result = Convert.ToBase64String(data.Where(s=>s.Image1!=null).OrderBy(s => s.Ordinal).Select(s => s.Image1).First().ToArray());
            return result;
        }

        private object GetInforProduct(string id)
        {
            var data = _context.Products.Where(s => s.Status != false && s.ProductID == id)
                    .Select(s => new
                    {
                        s.ProductID,
                        s.DisplayName,
                        s.SellPrice,
                        s.ProductDiscount.DiscountPercent,
                        Categorize = s.Categorize.DisplayName,
                        SalePrice = s.SellPrice * (decimal)(100 - s.ProductDiscount.DiscountPercent) / 100,
                        image = CheckExitImage(s.ProductID)
                    }).FirstOrDefault();
            return data;
        }
    }
}