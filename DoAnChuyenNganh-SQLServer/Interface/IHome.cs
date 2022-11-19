using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DoAnChuyenNganh.Interface
{
    public interface IHome
    {
        /// <summary>
        /// Show all list categorize
        /// </summary>
        /// <returns></returns>
        List<Categorize> ListCategorize();

        List<Supplier> ListBrand(string id);
    }
}