using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using doanweb.PublicClass;

namespace doanweb.Controllers
{
    public class Admin_QLNSXController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();

        public ActionResult QuanLyNhaSanXuat()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstnsx = db.tbl_nhasanxuat.ToList();
            return View(lstnsx);
        }

        [HttpGet]
        public ActionResult QuanLyNhaSanXuat_Them()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLNSX_T_TieuDe = null;
            ViewBag.QLNSX_T_TenNhaSanXuat = null;
            ViewBag.QLNSX_T_NuocSanXuat = null;

            return View();
        }
        [HttpPost]
        public ActionResult QuanLyNhaSanXuat_Them(tbl_nhasanxuat f)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }


            if(f.TenNhaSanXuat==null)
            {
                ViewBag.QLNSX_T_TenNhaSanXuat = "( Tên Nhà Sản Xuất Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLNSX_T_TenNhaSanXuat = null;
            }

            if(f.NuocSanXuat==null)
            {
                ViewBag.QLNSX_T_NuocSanXuat = "( Nước Sản Xuất Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLNSX_T_NuocSanXuat = null;
            }

            if(ViewBag.QLNSX_T_TenNhaSanXuat!=null || ViewBag.QLNSX_T_NuocSanXuat !=null)
            {
                return View();
            }

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(f.TenNhaSanXuat);
            if(ketqua==true)
            {
                ViewBag.QLNSX_T_TenNhaSanXuat = null;
            }
            else
            {
                ViewBag.QLNSX_T_TenNhaSanXuat = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            ketqua = ktc.hamcatchuoi(f.NuocSanXuat);
            if (ketqua == true)
            {
                ViewBag.QLNSX_T_NuocSanXuat = null;
            }
            else
            {
                ViewBag.QLNSX_T_NuocSanXuat = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if (ViewBag.QLNSX_T_TenNhaSanXuat != null || ViewBag.QLNSX_T_NuocSanXuat != null)
            {
                return View();
            }

            string tennhasanxuat = f.TenNhaSanXuat.ToLower();
            string nuocsanxuat = f.NuocSanXuat.ToLower();
            var ktnsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.TenNhaSanXuat.ToLower() == tennhasanxuat && n.NuocSanXuat.ToLower() == nuocsanxuat);
            if(ktnsx!=null)
            {
                ViewBag.QLNSX_T_TieuDe = "( NHÀ SẢN XUẤT ĐÃ TỒN TẠI )";
                return View();
            }
            else
            {
                db.tbl_nhasanxuat.Add(f);
                db.SaveChanges();
                ViewBag.QLNSX_T_TieuDe = "( THÊM NHÀ SẢN XUẤT THÀNH CÔNG )";
                return View();
            }
        }

        public ActionResult QuanLyNhaSanXuat_Xoa(int MaXoa=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            tbl_nhasanxuat nsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.Id == MaXoa);
            if(nsx!=null)
            {
                var lstsp = db.tbl_sanpham.Where(n => n.NhaSanXuat == MaXoa).ToList();
                if(lstsp.Count!=0)
                {
                    foreach(var itemsp in lstsp)
                    {
                        var lstasp = db.tbl_anhchitietsanpham.Where(n => n.Id_Sp == itemsp.Id).ToList();
                        if(lstasp.Count!=0)
                        {
                            foreach (var itemasp in lstasp)
                            {
                                db.tbl_anhchitietsanpham.Remove(itemasp);

                            }
                        }
                        

                        var lstgh = db.tbl_giohang.Where(n=>n.Id_MaSanPham==itemsp.Id && (n.TrangThai==1 || n.TrangThai==2) ).ToList();
                        if(lstgh.Count!=0)
                        {
                            foreach (var itemgh in lstgh)
                            {
                                db.tbl_giohang.Remove(itemgh);
                            }
                        }
                        db.tbl_sanpham.Remove(itemsp);
                    }
                }
                db.tbl_nhasanxuat.Remove(nsx);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLyNhaSanXuat","Admin_QLNSX");
        }

        [HttpGet]
        public ActionResult QuanLyNhaSanXuat_Sua(int MaSua = 0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLNSX_S_TieuDe = null;
            ViewBag.QLNSX_S_TenNhaSanXuat = null;
            ViewBag.QLNSX_S_NuocSanXuat = null;

            var lstqlnsx = db.tbl_nhasanxuat.FirstOrDefault(n=>n.Id==MaSua);
            return View(lstqlnsx);
        }
        [HttpPost]
        public ActionResult QuanLyNhaSanXuat_Sua(tbl_nhasanxuat f,int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            if (f.TenNhaSanXuat == null)
            {
                ViewBag.QLNSX_S_TenNhaSanXuat = "( Tên Nhà Sản Xuất Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLNSX_S_TenNhaSanXuat = null;
            }

            if (f.NuocSanXuat == null)
            {
                ViewBag.QLNSX_S_NuocSanXuat = "( Nước Sản Xuất Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLNSX_S_NuocSanXuat = null;
            }

            if (ViewBag.QLNSX_S_TenNhaSanXuat != null || ViewBag.QLNSX_S_NuocSanXuat != null)
            {
                var lstqlnsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.Id == MaSua);
                return View(lstqlnsx);
            }

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(f.TenNhaSanXuat);
            if (ketqua == true)
            {
                ViewBag.QLNSX_S_TenNhaSanXuat = null;
            }
            else
            {
                ViewBag.QLNSX_S_TenNhaSanXuat = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            ketqua = ktc.hamcatchuoi(f.NuocSanXuat);
            if (ketqua == true)
            {
                ViewBag.QLNSX_S_NuocSanXuat = null;
            }
            else
            {
                ViewBag.QLNSX_S_NuocSanXuat = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if (ViewBag.QLNSX_S_TenNhaSanXuat != null || ViewBag.QLNSX_S_NuocSanXuat != null)
            {
                var lstqlnsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.Id == MaSua);
                return View(lstqlnsx);
            }

            string tennhasanxuat = f.TenNhaSanXuat.ToLower();
            string nuocsanxuat = f.NuocSanXuat.ToLower();
            var ktnsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.TenNhaSanXuat.ToLower() == tennhasanxuat && n.NuocSanXuat.ToLower() == nuocsanxuat && n.Id!=MaSua);
            if (ktnsx != null)
            {
                ViewBag.QLNSX_S_TieuDe = "( NHÀ SẢN XUẤT ĐÃ TỒN TẠI )";
                var lstqlnsx = db.tbl_nhasanxuat.FirstOrDefault(n => n.Id == MaSua);
                return View(lstqlnsx);
            }
            else
            {
                tbl_nhasanxuat sua = db.tbl_nhasanxuat.FirstOrDefault(n => n.Id == MaSua);
                sua.TenNhaSanXuat = f.TenNhaSanXuat;
                sua.NuocSanXuat = f.NuocSanXuat;

                db.SaveChanges();
                ViewBag.QLNSX_S_TieuDe = "(CẬP NHẬT NHÀ SẢN XUẤT THÀNH CÔNG )";
                return View(sua);
            }

        }
    }
}
