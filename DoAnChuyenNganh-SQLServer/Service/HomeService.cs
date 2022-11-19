using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh_SQLServer.Models;

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
        public List<Supplier> ListBrand(String id)
        {
            var data = _context.Categorizes.Where(s => s.CategorizeID == id)
                .Include(s => s.Products);
            throw new NotImplementedException();
        }

        /// <summary>
        /// code show categorize
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Categorize> ListCategorize()
        {
            var data =  _context.Categorizes.Where(s=>s.Status != false).ToList();
            return data;
        }
    }
}