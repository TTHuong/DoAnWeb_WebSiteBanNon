using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doanweb.Controllers
{
    public class DangXuatController : Controller
    {
        public ActionResult DangXuat()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            Session.Remove("taikhoan");
            Session.Remove("quyenhan");
            return RedirectToAction("Index", "Home"); 
        }

    }
}
