using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using doanweb.PublicClass;

namespace doanweb.Controllers
{
    public class Admin_QLGHController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();

        public ActionResult QuanLyGioHang()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstgh_qlgh=(from sp in db.tbl_sanpham.AsEnumerable()
                       from gh in db.tbl_giohang.AsEnumerable()
                       from kh in db.tbl_member.AsEnumerable()
                       where gh.Id_user==kh.Id && gh.Id_MaSanPham==sp.Id && gh.TrangThai==2
                       select new doanweb.Models.tbl_giohang
                       {
                           Id=gh.Id,
                           Id_user=gh.Id_user,
                           Id_MaSanPham=gh.Id_MaSanPham,
                           SoLuongSanPham=gh.SoLuongSanPham,
                           NgayNhap=gh.NgayNhap,
                           TrangThai=gh.TrangThai,
                           TenKhachHang=kh.TenKhachHang,
                           AnhSanPham=sp.AnhSanPham,
                           TenSanPham=sp.TenSanPham
                       }
                
                ).ToList();
            return View(lstgh_qlgh);
        }

        public ActionResult QuanLyGioHang_DonHangThanhCong()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstgh_qlgh = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              from kh in db.tbl_member.AsEnumerable()
                              where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 3
                              select new doanweb.Models.tbl_giohang
                              {
                                  Id = gh.Id,
                                  Id_user = gh.Id_user,
                                  Id_MaSanPham = gh.Id_MaSanPham,
                                  SoLuongSanPham = gh.SoLuongSanPham,
                                  NgayNhap = gh.NgayNhap,
                                  TrangThai = gh.TrangThai,
                                  TenKhachHang = kh.TenKhachHang,
                                  AnhSanPham = sp.AnhSanPham,
                                  TenSanPham = sp.TenSanPham
                              }

                ).ToList();

            return View(lstgh_qlgh);
        }

        public ActionResult QuanLyGioHang_DonHangThatBai()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstgh_qlgh = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              from kh in db.tbl_member.AsEnumerable()
                              where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 4
                              select new doanweb.Models.tbl_giohang
                              {
                                  Id = gh.Id,
                                  Id_user = gh.Id_user,
                                  Id_MaSanPham = gh.Id_MaSanPham,
                                  SoLuongSanPham = gh.SoLuongSanPham,
                                  NgayNhap = gh.NgayNhap,
                                  TrangThai = gh.TrangThai,
                                  TenKhachHang = kh.TenKhachHang,
                                  AnhSanPham = sp.AnhSanPham,
                                  TenSanPham = sp.TenSanPham
                              }

                ).ToList();

            return View(lstgh_qlgh);
        }

        public ActionResult QuanLyGioHang_Xoa(int MaXoa=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var ghx = db.tbl_giohang.Where(n=>n.TrangThai==2).FirstOrDefault(n => n.Id == MaXoa);
            if(ghx!=null )
            {
                db.tbl_giohang.Remove(ghx);

                /////////////////////////////////////////////////////////////////
                int taikhoan = Int32.Parse(Session["taikhoan"].ToString());
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == ghx.Id_user);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == ghx.Id_MaSanPham);
                var adlichsu = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);

                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = ghx.SoLuongSanPham;
                moi.NgayNhap = ghx.NgayNhap;
                moi.TrangThai = "Sản Phẩm Đang Chờ Thanh Toán - Admin";
                moi.TenNguoiThayDoi = adlichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Xóa Giỏ Hàng Đang Chờ Thanh Toán - Admin";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////


                db.SaveChanges();
            }
            return RedirectToAction("QuanLyGioHang", "Admin_QLGH");
        }

        [HttpGet]
        public ActionResult QuanLyGioHang_Sua(int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var gh_qlgh = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              from kh in db.tbl_member.AsEnumerable()
                              where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 2
                              select new doanweb.Models.tbl_giohang
                              {
                                  Id = gh.Id,
                                  Id_user = gh.Id_user,
                                  Id_MaSanPham = gh.Id_MaSanPham,
                                  SoLuongSanPham = gh.SoLuongSanPham,
                                  NgayNhap = gh.NgayNhap,
                                  TrangThai = gh.TrangThai,
                                  TenKhachHang = kh.TenKhachHang,
                                  AnhSanPham = sp.AnhSanPham,
                                  TenSanPham = sp.TenSanPham
                              }

                ).FirstOrDefault();

            return View(gh_qlgh);
        }

        [HttpPost]
        public ActionResult QuanLyGioHang_Sua(FormCollection f,int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            int giatri =Convert.ToInt32( f.Get("TrangThai_S"));

            if(giatri!=4 && giatri!=3)
            {
                ViewBag.QLGH_S_TieuDe = "( XẢY RA LỖI GIÁ TRỊ TRANG THÁI ! VUI LÒNG CHỌN LẠI ĐƠN HÀNG )";
                var gh_qlgh = (from sp in db.tbl_sanpham.AsEnumerable()
                                  from gh in db.tbl_giohang.AsEnumerable()
                                  from kh in db.tbl_member.AsEnumerable()
                                  where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 2
                                  select new doanweb.Models.tbl_giohang
                                  {
                                      Id = gh.Id,
                                      Id_user = gh.Id_user,
                                      Id_MaSanPham = gh.Id_MaSanPham,
                                      SoLuongSanPham = gh.SoLuongSanPham,
                                      NgayNhap = gh.NgayNhap,
                                      TrangThai = gh.TrangThai,
                                      TenKhachHang = kh.TenKhachHang,
                                      AnhSanPham = sp.AnhSanPham,
                                      TenSanPham = sp.TenSanPham
                                  }

                ).FirstOrDefault();

                return View(gh_qlgh);
            }
            else
            {
                ViewBag.QLGH_S_TieuDe = null;
            }

            var ktgh_s = db.tbl_giohang.FirstOrDefault(n => n.Id == MaSua);
            if(ktgh_s!=null)
            {
                

                ktgh_s.TrangThai = giatri;


                /////////////////////////////////////////////////////////////////
                int taikhoan = Int32.Parse(Session["taikhoan"].ToString());
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == ktgh_s.Id_user);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == ktgh_s.Id_MaSanPham);
                var adlichsu = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);

                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = ktgh_s.SoLuongSanPham;
                moi.NgayNhap = ktgh_s.NgayNhap;
                
                if(giatri==3)
                {
                    moi.TrangThai = "Sản Phẩm Đã Thanh Toán Thành Công - Admin";
                }
                else
                {
                    moi.TrangThai = "Sản Phẩm Đã Thanh Toán Thất Bại - Admin";
                }
                moi.TenNguoiThayDoi = adlichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Sửa Giỏ Hàng Đang Chờ Thanh Toán - Admin";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////


                db.SaveChanges();
                
                return RedirectToAction("QuanLyGioHang", "Admin_QLGH");
            }
            else
            {
                var lstgh_qlgh = (from sp in db.tbl_sanpham.AsEnumerable()
                                  from gh in db.tbl_giohang.AsEnumerable()
                                  from kh in db.tbl_member.AsEnumerable()
                                  where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 2
                                  select new doanweb.Models.tbl_giohang
                                  {
                                      Id = gh.Id,
                                      Id_user = gh.Id_user,
                                      Id_MaSanPham = gh.Id_MaSanPham,
                                      SoLuongSanPham = gh.SoLuongSanPham,
                                      NgayNhap = gh.NgayNhap,
                                      TrangThai = gh.TrangThai,
                                      TenKhachHang = kh.TenKhachHang,
                                      AnhSanPham = sp.AnhSanPham,
                                      TenSanPham = sp.TenSanPham
                                  }

                ).FirstOrDefault();
                ViewBag.QLGH_S_TieuDe = "( CẬP NHẬT KHÔNG THÀNH CÔNG )";
                return View(lstgh_qlgh);
            }
        }

    }
}
