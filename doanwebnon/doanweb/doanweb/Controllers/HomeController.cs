using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;

namespace doanweb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult sliderpartial()
        {
            var list = db.tbl_slider.OrderByDescending(n => n.Id).Where(n=>n.TrangThai==2).ToList();
            return PartialView(list);
        }
        public PartialViewResult ContentPartial()
        {
            var list = db.tbl_sanpham.OrderByDescending(n=>n.Id).Take(21).ToList();
            return PartialView(list);
        }
    }
}
