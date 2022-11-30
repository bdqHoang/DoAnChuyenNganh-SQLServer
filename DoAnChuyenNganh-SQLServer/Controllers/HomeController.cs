using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh.Service;
using DoAnChuyenNganh_SQLServer.Service;
using Newtonsoft.Json;

namespace DoAnChuyenNganh.Controllers
{
    public class HomeController : Controller
    {
        private IHome _homeService = new HomeService();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Show menu categorize
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/home/showcategorize")]
        public JsonResult ShowCategorize()
        {
            var data = _homeService.ListCategorize();
            var suplier = _homeService.ListSuppliers();
            return Json(new {data = data, suplier = suplier}, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// show supplier for categorize
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/home/getsupplier")]
        public JsonResult GetSupplier(string id)
        {
            var data = _homeService.ListBrand(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/topcategories")]
        public JsonResult TopCategories()
        {
            var data = _homeService.ListTopCategories();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/listsaleproduct")]
        public JsonResult ListSaleProduct()
        {
            var data = _homeService.ListSaleProduct();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/topproductbestsaler")]
        public JsonResult TopProductBestSaler() {
            var data = _homeService.ProductBestSaler();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/producttredding")]
        public JsonResult ProductTredding() 
        {
            var data = _homeService.TrenddingItem();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/newarrival")]
        public JsonResult NewArrival() 
        { 
            var data = _homeService.NewArrival();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/getinformation")]
        public JsonResult GetInformation(string id) {
            var data = _homeService.DetailProduct(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/recommendforyou")]
        public JsonResult RecommendForYou(string UserID)
        {
            var data = _homeService.RecommentProduct(UserID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("~/home/getinformation")]
        public JsonResult GetInformation(string id) {
            var data = _homeService.DetailProduct(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Shop()
        {
            return View();
        }

        public ActionResult DetailProduct(string id)
        {
            var data = _homeService.GetProduct(id);
            return View(data);
        }

        [HttpGet]
        [Route("~/home/getquantity")]
        public JsonResult GetQuantity(string productid, string colorID = null, string optioID = null)
        {
            var data = _homeService.Quantity(productid, colorID, optioID);
            if(data == -1)
            {
                return Json("Product will update soon", JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WishList() 
        {
            return View();
        }
    }
}