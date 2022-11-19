using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh_SQLServer.Models;

namespace DoAnChuyenNganh.Service
{
    public class HomeService : IHome
    {
        private GearShopDataContext _context = new GearShopDataContext();
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