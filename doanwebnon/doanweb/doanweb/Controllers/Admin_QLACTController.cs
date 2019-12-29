using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using doanweb.PublicClass;
using System.IO;
namespace doanweb.Controllers
{
    public class Admin_QLACTController : Controller
    {

        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();

        public ActionResult QuanLyAnhChiTiet()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstact=(from sp in db.tbl_sanpham.AsEnumerable()
                        from act in db.tbl_anhchitietsanpham.AsEnumerable()
                        where sp.Id==act.Id_Sp
                        select new doanweb.Models.tbl_anhchitietsanpham
                        {
                            Id=act.Id,
                            TenSanPham=sp.TenSanPham,
                            AnhSanPham=sp.AnhSanPham,
                            DuongDan=act.DuongDan
                        }
                ).ToList();
            return View(lstact);
        }

        [HttpGet]
        public ActionResult QuanLyAnhChiTiet_Them()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLACT_T_AnhChiTiet = null;
            
            ViewBag.QLACT_T_TenSanPham = null;
            ViewBag.QLACT_T_TieuDe = null;

            List<tbl_sanpham> sp = db.tbl_sanpham.ToList();
            SelectList lstsp = new SelectList(sp, "Id", "TenSanPham");
            ViewBag.ListSanPham = lstsp;

            return View();
        }
        [HttpPost]
        public ActionResult QuanLyAnhChiTiet_Them(tbl_anhchitietsanpham f, HttpPostedFileBase DuongDan)
        {
            List<tbl_sanpham> sp = db.tbl_sanpham.ToList();
            SelectList lstsp = new SelectList(sp, "Id", "TenSanPham");
            ViewBag.ListSanPham = lstsp;

            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }


            if (DuongDan == null || DuongDan.ContentLength == 0)
            {
                ViewBag.QLACT_T_AnhChiTiet = "( Ảnh Chi Tiết Số 1 Không Được Bỏ Trống )";
            }
            else
            {
                if (DuongDan.ContentLength > 104857600)
                {
                    ViewBag.QLACT_T_AnhChiTiet = "( Kích Thước Ảnh Tối Đa 100MB )";
                }
                else
                {
                    ViewBag.QLACT_T_AnhChiTiet = null;
                }
            }

            
            var ktsp = db.tbl_sanpham.FirstOrDefault(n => n.Id == f.Id_Sp);
            if(ktsp!=null)
            {
                ViewBag.QLACT_T_TenSanPham = null;
            }
            else
            {
                ViewBag.QLACT_T_TenSanPham = "( Đã Xảy Ra Lỗi Khi Bạn Đang Cố Thay Đổi Code Html ! Vui Lòng Không Làm Vậy Nữa Và Load Lại Trang )";
            }

            if(ViewBag.QLACT_T_TenSanPham != null|| ViewBag.QLACT_T_AnhChiTiet != null )
            {
                return View();
            }

            int Idact = 0;
            var actmn = db.tbl_anhchitietsanpham.OrderByDescending(n => n.Id).FirstOrDefault();
            if (actmn != null)
            {
                Idact = actmn.Id;
            }
            string path = Server.MapPath("~/Content/images/chitietsanpham/");
            string ex = Path.GetExtension(DuongDan.FileName);
            string namefile = path + "anhchitiet" + (Idact + 1).ToString() + ex;
            if (!System.IO.File.Exists(namefile))
            {
                DuongDan.SaveAs(namefile);
            }
            f.DuongDan = "anhchitiet" + (Idact + 1).ToString() + ex;
            db.tbl_anhchitietsanpham.Add(f);
            db.SaveChanges();
            ViewBag.QLACT_T_TieuDe = "( THÊM ẢNH CHI TIẾT THÀNH CÔNG )";
            return View();
        }

        public ActionResult QuanLyAnhChiTiet_Xoa(int MaXoa=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var ktact = db.tbl_anhchitietsanpham.FirstOrDefault(n => n.Id == MaXoa);
            if(ktact!=null)
            {
                string path = Server.MapPath("~/Content/images/chitietsanpham/" + ktact.DuongDan);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                db.tbl_anhchitietsanpham.Remove(ktact);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyAnhChiTiet", "Admin_QLACT");
        }

        [HttpGet]
        public ActionResult QuanLyAnhChiTiet_Sua(int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLACT_S_AnhChiTiet = null;
            ViewBag.QLACT_S_TenSanPham = null;
            ViewBag.QLACT_S_TieuDe = null;

            List<tbl_sanpham> sp = db.tbl_sanpham.ToList();
            SelectList lstsp = new SelectList(sp, "Id", "TenSanPham");
            ViewBag.ListSanPham = lstsp;

            var actsua = db.tbl_anhchitietsanpham.FirstOrDefault(n => n.Id == MaSua);
            return View(actsua);
        }
        [HttpPost]
        public ActionResult QuanLyAnhChiTiet_Sua(tbl_anhchitietsanpham f,HttpPostedFileBase DuongDan,int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            List<tbl_sanpham> sp = db.tbl_sanpham.ToList();
            SelectList lstsp = new SelectList(sp, "Id", "TenSanPham");
            ViewBag.ListSanPham = lstsp;

            if (DuongDan == null||DuongDan.ContentLength == 0  )
            {
                ViewBag.QLACT_T_AnhChiTiet = null;
            }
            else
            {
                if (DuongDan.ContentLength > 104857600)
                {
                    ViewBag.QLACT_T_AnhChiTiet = "( Kích Thước Ảnh Tối Đa 100MB )";
                }
                else
                {
                    ViewBag.QLACT_T_AnhChiTiet = null;
                }
            }


            var ktsp = db.tbl_sanpham.FirstOrDefault(n => n.Id == f.Id_Sp);
            if (ktsp != null)
            {
                ViewBag.QLACT_T_TenSanPham = null;
            }
            else
            {
                ViewBag.QLACT_T_TenSanPham = "( Đã Xảy Ra Lỗi Khi Bạn Đang Cố Thay Đổi Code Html ! Vui Lòng Không Làm Vậy Nữa Và Load Lại Trang )";
            }

            if (ViewBag.QLACT_T_TenSanPham != null || ViewBag.QLACT_T_AnhChiTiet != null)
            {
                var actsua = db.tbl_anhchitietsanpham.FirstOrDefault(n => n.Id == MaSua);
                return View(actsua);
            }

            var sua = db.tbl_anhchitietsanpham.FirstOrDefault(n => n.Id == MaSua);
            if(sua!=null)
            {
                if (DuongDan != null && DuongDan.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Content/images/chitietsanpham/");
                    if (System.IO.File.Exists(path + sua.DuongDan))
                    {
                        System.IO.File.Delete(path + sua.DuongDan);
                    }
                    string ex = Path.GetExtension(DuongDan.FileName);
                    string namefile = path + "anhchitiet" + (MaSua).ToString() + ex;
                    DuongDan.SaveAs(namefile);

                    sua.DuongDan = "anhchitiet" + (MaSua).ToString() + ex;
                }
                if(f.Id_Sp!=null)
                {
                    sua.Id_Sp = f.Id_Sp;
                }
                db.SaveChanges();
                ViewBag.QLACT_S_TieuDe = "(CẬP NHẬT ẢNH CHI TIẾT SẢN PHẨM THÀNH CÔNG )";
                return View(sua);
            }
            else
            {
                ViewBag.QLACT_S_TieuDe = "(CẬP NHẬT ẢNH CHI TIẾT SẢN PHẨM THẤT BẠI )";
                var actsua = db.tbl_anhchitietsanpham.FirstOrDefault(n => n.Id == MaSua);
                return View(actsua);
            }

        }
    }
}
