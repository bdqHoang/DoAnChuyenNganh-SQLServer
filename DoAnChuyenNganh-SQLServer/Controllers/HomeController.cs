using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoAnChuyenNganh.Interface;
using DoAnChuyenNganh.Service;
using DoAnChuyenNganh_SQLServer.Models;

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
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Shop()
        {
            return View();
        }
    }
}