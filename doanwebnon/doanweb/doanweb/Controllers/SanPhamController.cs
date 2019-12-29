using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
namespace doanweb.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        public ViewResult BaoLoiTrang()
        {
            return View();
        }
        public ActionResult XemChiTiet(int MaSanPham=0)
        {
            tbl_sanpham sanpham = db.tbl_sanpham.SingleOrDefault(n => n.Id == MaSanPham);
            if (sanpham == null)
            {
                return RedirectToAction("BaoLoiTrang","SanPham");
            }
            return View(sanpham);
        }

        public PartialViewResult AnhXemChiTietSanPham(int MaSanPham=0)
        {
            var anhchitiet = db.tbl_anhchitietsanpham.OrderByDescending(n => n.Id).Where(n => n.Id_Sp == MaSanPham).ToList();
            return PartialView(anhchitiet);
        }
        public PartialViewResult PreviewThumnailXCTPartial(int MaSanPham=0)
        {
            var anhchitiet = db.tbl_anhchitietsanpham.OrderByDescending(n => n.Id).Where(n => n.Id_Sp == MaSanPham).Take(5).ToList();
            return PartialView(anhchitiet);
        }
        public ViewResult TatCaSanPham()
        {
            var list = db.tbl_sanpham.OrderByDescending(n => n.Id).ToList();
            return View(list);
        }

    }
}
