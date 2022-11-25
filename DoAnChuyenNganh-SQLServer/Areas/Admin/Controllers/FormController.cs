using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnChuyenNganh_SQLServer.Areas.Admin.Controllers
{
    public class FormController : Controller
    {
        // GET: Admin/Form
        public ActionResult Create()
        {
            return PartialView();
        }
    }
}