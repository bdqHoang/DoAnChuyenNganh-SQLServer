using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web.Helpers;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh_SQLServer.Models;
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












        //------------------------------function-------------------------------------------------
        //check exit image
        private string CheckExitImage(string product)
        {
            var data = _context.Images.Where(n => n.ProductID == product);
            
            if (data.Count() ==0)
            {
                return null;
            }
            var result = Convert.ToBase64String(data.OrderBy(s => s.Ordinal).Select(s => s.Image1).First().ToArray());
            return result;
        }
    }
}