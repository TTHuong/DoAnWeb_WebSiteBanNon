using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;

namespace doanweb.Controllers
{
    public class Admin_QLLICHSUController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        public ActionResult LichSuQuanLyNguoiDung()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstnd = db.tbl_Hmember.OrderByDescending(n=>n.Id).ToList();
            return View(lstnd);
        }

        public ActionResult LichSuQuanLyGioHang()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstgh = db.tbl_Hgiohang.OrderByDescending(n => n.Id).ToList();
            return View(lstgh);
        }

    }
}
