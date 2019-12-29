using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;

namespace doanweb.Controllers
{
    public class TimKiemSpController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        
        [HttpGet]
        public ActionResult KetQuaTimKiem(string txttimkiem)
        {
            ViewBag.key = txttimkiem;
            var lstTimKiem = db.tbl_sanpham.OrderByDescending(n => n.Id).Where(n => n.TenSanPham.Contains(txttimkiem)).ToList();
            if(lstTimKiem.Count==0)
            {
                return View(db.tbl_sanpham.OrderByDescending(n => n.Id).ToList());
            }
            ViewBag.soluongketqua = lstTimKiem.Count;
            return View(lstTimKiem);
        }
    }
}
