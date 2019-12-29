using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
namespace doanweb.Controllers
{
    public class LayoutController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();

        public PartialViewResult SlideFooterPartial()
        {
            var list = db.tbl_slider.OrderByDescending(n => n.Id).Where(n => n.TrangThai == 2).ToList();
            return PartialView(list);
        }
        public PartialViewResult NhaSanXuatDongHoNamPartial()
        {
            //var list = db.tbl_nhasanxuat.ToList();
            var list=(from sp in db.tbl_sanpham.AsEnumerable()
                      from nxb in db.tbl_nhasanxuat.AsEnumerable()
                      where nxb.Id==sp.NhaSanXuat && sp.LoaiSanPham=="Nón Nam"
                      select new doanweb.Models.tbl_nhasanxuat
                      {
                          Id=nxb.Id,
                          TenNhaSanXuat=nxb.TenNhaSanXuat,
                          NuocSanXuat=nxb.NuocSanXuat
                      }
                ).ToList();
            return PartialView(list);
        }
        public PartialViewResult NhaSanXuatDongHoNuPartial()
        {
            var kq = db.tbl_nhasanxuat.GroupBy(n => n.TenNhaSanXuat).ToList();

            //var list = db.tbl_nhasanxuat.ToList();
            var list = (from sp in db.tbl_sanpham.AsEnumerable()
                        from nxb in db.tbl_nhasanxuat.AsEnumerable()
                        where nxb.Id == sp.NhaSanXuat && sp.LoaiSanPham == "Nón Nữ"
                        select new doanweb.Models.tbl_nhasanxuat
                        {
                            Id = nxb.Id,
                            TenNhaSanXuat = nxb.TenNhaSanXuat,
                            NuocSanXuat = nxb.NuocSanXuat
                        }
                ).ToList();
            
            return PartialView(list);
        }
        public PartialViewResult NhaSanXuatDongHoDoiPartial()
        {
            //var list = db.tbl_nhasanxuat.ToList();
            var list = (from sp in db.tbl_sanpham.AsEnumerable()
                        from nxb in db.tbl_nhasanxuat.AsEnumerable()
                        where nxb.Id == sp.NhaSanXuat && sp.LoaiSanPham == "Nón Cặp"
                        select new doanweb.Models.tbl_nhasanxuat
                        {
                            Id = nxb.Id,
                            TenNhaSanXuat = nxb.TenNhaSanXuat,
                            NuocSanXuat = nxb.NuocSanXuat
                        }
                ).ToList();
            return PartialView(list);
        }
        public ActionResult XemTatCaSanPhamCuaCungNhaSanXuat(int NSX=0,string loai="")
        {
            var sp = db.tbl_sanpham.OrderByDescending(n => n.Id).Where(n => n.NhaSanXuat == NSX ).Where(n=>n.LoaiSanPham==loai).ToList();
            if(sp.Count==0)
            {
                return RedirectToAction("BaoLoiTrang","SanPham");
            }
            return View(sp);
        }
        public ActionResult XemSanPhamTheoGia(int Gia=0,int MucDo=0)
        {
            List<tbl_sanpham> list=null;
            if(MucDo==1)
            {
                list = db.tbl_sanpham.OrderByDescending(n => n.Id).Where(n => n.GiaTien <= Gia).ToList();
            }
            else if(MucDo==2)
            {
                list = db.tbl_sanpham.OrderByDescending(n => n.Id).Where(n => n.GiaTien > Gia).ToList();
            }
            try
            {
                if (list.Count == 0 || (MucDo != 1 && MucDo != 2) || Gia < 0)
                {
                    return RedirectToAction("BaoLoiTrang", "SanPham");
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("BaoLoiTrang", "SanPham");
            }
            return View(list);
            
        }


        /////////////////////////////////////////////layout admin////////////////////////////////////////////////////

        public ActionResult ThongBaoDonHangMoi_Admin_PartialView()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lst = (from sp in db.tbl_sanpham.AsEnumerable()
                       from kh in db.tbl_member.AsEnumerable()
                       from gh in db.tbl_giohang.AsEnumerable()
                       where gh.Id_user == kh.Id && gh.Id_MaSanPham == sp.Id && gh.TrangThai == 2
                       select new doanweb.Models.tbl_giohang
                       {
                           Id=gh.Id,
                           TenKhachHang = kh.TenKhachHang,
                           TenSanPham = sp.TenSanPham
                       }).Take(6).OrderByDescending(n=>n.Id).ToList();
            return PartialView(lst);
        }

        public ActionResult TenAdmin_PartialView()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            int tk = Int32.Parse(Session["taikhoan"].ToString());
            tbl_member kh = db.tbl_member.FirstOrDefault(n => n.Id == tk);
            return PartialView(kh);
        }

    }
}
