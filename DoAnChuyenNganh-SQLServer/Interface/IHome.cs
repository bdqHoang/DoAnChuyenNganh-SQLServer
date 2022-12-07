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
        object ListSuppliers();
        /// <summary>
        /// Show all list categorize
        /// </summary>
        /// <returns></returns>
        object ListCategorize();
        /// <summary>
        /// Show list supplier have product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object ListBrand(string id);
        /// <summary>
        /// show top categorize
        /// </summary>
        /// <returns></returns>
        object ListTopCategories();
        /// <summary>
        /// show sale product
        /// </summary>
        /// <returns></returns>
        object ListSaleProduct();
        /// <summary>
        /// product best saler
        /// </summary>
        /// <returns></returns>
        object ProductBestSaler();
        /// <summary>
        /// show item trendding
        /// </summary>
        /// <returns></returns>
        object TrenddingItem();
        /// <summary>
        /// show new product
        /// </summary>
        /// <returns></returns>
        object NewArrival();
        /// <summary>
        /// show detail product
        /// </summary>
        /// <returns></returns>
        object DetailProduct(string id);
        /// <summary>
        /// get information product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProduct(string id);
        int Quantity(string productid, string colorID = null, string optioID = null);

        object RecommentProduct(string customerID);
    }
}