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
    public class Admin_QLSLIDEController : Controller
    {

        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();

        public ActionResult QuanLySlide()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstslide = db.tbl_slider.ToList();
            return View(lstslide);
        }
        [HttpGet]
        public ActionResult QuanLySlide_Them()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLSLIDE_T_TieuDe = null;
            ViewBag.QLSLIDE_T_TenSlide = null;
            ViewBag.QLSLIDE_T_NoiDungChiTiet = null;
            ViewBag.QLSLIDE_T_AnhSlide = null;
            ViewBag.QLSLIDE_T_TrangThai = null;

            return View();
        }
        [HttpPost]
        public ActionResult QuanLySlide_Them(FormCollection f,HttpPostedFileBase DuongDan )
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            if(DuongDan==null || DuongDan.ContentLength==0)
            {
                ViewBag.QLSLIDE_T_AnhSlide = "( Ảnh Slide Không Được Bỏ Trống )";
            }
            else
            {
                if (DuongDan.ContentLength > 104857600)
                {
                    ViewBag.QLSLIDE_T_AnhSlide = "( Kích Thước Ảnh Tối Đa 100MB )";
                }
                else
                {
                    ViewBag.QLSLIDE_T_AnhSlide = null;
                }
                
            }

            int trangthai= Convert.ToInt32(f.Get("TrangThai_T"));
            if(trangthai!=1 && trangthai!=2)
            {
                ViewBag.QLSLIDE_T_TrangThai = "( Bạn Đang Cố Thay Đổi Code Html ! Vui Lòng Không Làm Vậy Nữa Và Load Lại Trang )";
            }
            else
            {
                ViewBag.QLSLIDE_T_TrangThai = null;
            }

            if(f.Get("TenSlider")!=null)
            {
                bool ketqua = ktc.hamcatchuoi(f.Get("TenSlider"));
                if(ketqua==true)
                {
                    ViewBag.QLSLIDE_T_TenSlide = null;
                }
                else
                {
                    ViewBag.QLSLIDE_T_TenSlide = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            else
            {
                ViewBag.QLSLIDE_T_TenSlide = null;
            }

            if (f.Get("NoiDungChiTiet") != null)
            {
                bool ketqua = ktc.hamcatchuoi(f.Get("NoiDungChiTiet"));
                if (ketqua == true)
                {
                    ViewBag.QLSLIDE_T_NoiDungChiTiet = null;
                }
                else
                {
                    ViewBag.QLSLIDE_T_NoiDungChiTiet = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            else
            {
                ViewBag.QLSLIDE_T_NoiDungChiTiet = null;
            }

            if(ViewBag.QLSLIDE_T_TenSlide != null || ViewBag.QLSLIDE_T_NoiDungChiTiet != null || ViewBag.QLSLIDE_T_AnhSlide != null || ViewBag.QLSLIDE_T_TrangThai != null)
            {
                return View();
            }
            int Idsl = 0;
            var slmn = db.tbl_slider.OrderByDescending(n => n.Id).FirstOrDefault();
            if (slmn != null)
            {
                Idsl = slmn.Id;
            }
            string path = Server.MapPath("~/Content/images/slider/");
            string ex = Path.GetExtension(DuongDan.FileName);
            string namefile = path + "slide" + (Idsl + 1).ToString() + ex;

            tbl_slider sl = new tbl_slider();
            if(f.Get("TenSlider")!=null)
            {
                sl.TenSlider = f.Get("TenSlider");
            }
            if(f.Get("NoiDungChiTiet")!=null)
            {
                sl.NoiDungChiTiet = f.Get("NoiDungChiTiet");
            }
            if (!System.IO.File.Exists(namefile))
            {
                DuongDan.SaveAs(namefile);
            }

            sl.DuongDan = "slide" + (Idsl + 1).ToString() + ex;
            sl.TrangThai =Convert.ToInt32( f.Get("TrangThai_T"));
            db.tbl_slider.Add(sl);
            db.SaveChanges();
            ViewBag.QLSLIDE_T_TieuDe = "( THÊM SLIDE THÀNH CÔNG )";
            return View();

        }

        public ActionResult QuanLySlide_Xoa(int MaXoa=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var ktsl = db.tbl_slider.FirstOrDefault(n => n.Id == MaXoa);
            if(ktsl!=null)
            {
                string path = Server.MapPath("~/Content/images/slider/" + ktsl.DuongDan);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                db.tbl_slider.Remove(ktsl);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLySlide", "Admin_QLSLIDE");
        }

        [HttpGet]

        public ActionResult QuanLySlide_Sua(int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.QLSLIDE_S_TieuDe = null;
            ViewBag.QLSLIDE_S_TenSlide = null;
            ViewBag.QLSLIDE_S_NoiDungChiTiet = null;
            ViewBag.QLSLIDE_S_AnhSlide = null;
            ViewBag.QLSLIDE_S_TrangThai = null;

            var ktsl = db.tbl_slider.FirstOrDefault(n => n.Id == MaSua);
            return View(ktsl);
        }

        [HttpPost]
        public ActionResult QuanLySlide_Sua(tbl_slider f,FormCollection fr, HttpPostedFileBase DuongDan, int MaSua = 0)
        {
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
                ViewBag.QLSLIDE_S_AnhSlide = null;
            }
            else
            {
                if (DuongDan.ContentLength > 104857600)
                {
                    ViewBag.QLSLIDE_S_AnhSlide = "( Kích Thước Ảnh Tối Đa 100MB )";
                }
                else
                {
                    ViewBag.QLSLIDE_S_AnhSlide = null;
                }

            }

            int trangthai =Convert.ToInt32( fr.Get("TrangThai_T"));
            if (trangthai != 1 && trangthai != 2)
            {
                ViewBag.QLSLIDE_S_TrangThai = "( Bạn Đang Cố Thay Đổi Code Html ! Vui Lòng Không Làm Vậy Nữa Và Load Lại Trang )";
            }
            else
            {
                ViewBag.QLSLIDE_S_TrangThai = null;
            }

            if (f.TenSlider != null)
            {
                bool ketqua = ktc.hamcatchuoi(f.TenSlider);
                if (ketqua == true)
                {
                    ViewBag.QLSLIDE_S_TenSlide = null;
                }
                else
                {
                    ViewBag.QLSLIDE_S_TenSlide = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            else
            {
                ViewBag.QLSLIDE_S_TenSlide = null;
            }

            if (f.NoiDungChiTiet != null)
            {
                bool ketqua = ktc.hamcatchuoi(f.NoiDungChiTiet);
                if (ketqua == true)
                {
                    ViewBag.QLSLIDE_S_NoiDungChiTiet = null;
                }
                else
                {
                    ViewBag.QLSLIDE_S_NoiDungChiTiet = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            else
            {
                ViewBag.QLSLIDE_S_NoiDungChiTiet = null;
            }

            if (ViewBag.QLSLIDE_S_TenSlide != null || ViewBag.QLSLIDE_S_NoiDungChiTiet != null || ViewBag.QLSLIDE_S_AnhSlide != null || ViewBag.QLSLIDE_S_TrangThai != null)
            {
                var ktslide1 = db.tbl_slider.FirstOrDefault(n => n.Id == MaSua);
                return View(ktslide1);
            }

            var sua = db.tbl_slider.FirstOrDefault(n => n.Id == MaSua);
            if(sua!=null)
            {
                if(DuongDan!=null && DuongDan.ContentLength>0)
                {
                    string path = Server.MapPath("~/Content/images/slider/");
                    string ex = Path.GetExtension(DuongDan.FileName);
                    string namefile = path + "slide" + (MaSua).ToString() + ex;

                    if (System.IO.File.Exists(path+sua.DuongDan))
                    {
                        System.IO.File.Delete(path + sua.DuongDan);
                    }

                    DuongDan.SaveAs(namefile);
                    sua.DuongDan = "slide" + (MaSua).ToString() + ex;

                }
                if (f.TenSlider != null)
                {
                    sua.TenSlider = f.TenSlider;
                }
                if (f.NoiDungChiTiet != null)
                {
                    sua.NoiDungChiTiet = f.NoiDungChiTiet;
                }
                sua.TrangThai = Convert.ToInt32(trangthai);

                db.SaveChanges();
                ViewBag.QLSLIDE_S_TieuDe = "( CẬP NHẬT SLIDE THÀNH CÔNG )";
                return View(sua);
            }
            else
            {
                ViewBag.QLSLIDE_S_TieuDe = "( CẬP NHẬT SLIDE KHÔNG THÀNH CÔNG )";
                var ktslide2 = db.tbl_slider.FirstOrDefault(n => n.Id == MaSua);
                return View(ktslide2);
            }
        }
    }
}
