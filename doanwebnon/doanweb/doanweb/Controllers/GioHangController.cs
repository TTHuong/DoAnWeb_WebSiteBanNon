using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;

namespace doanweb.Controllers
{
    public class GioHangController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        
        //Hàm này dùng để xử lý cho view GioHang 
        public ActionResult GioHang()
        {
            //TTH1_start
            //dòng này dùng để kiểm tra khi người dùng bấm vào biểu tượng giỏ hàng nếu người ta chưa đăng nhập sẽ chuyển sang trang
            //đăng nhập và cái session["taikhoan"] này được lưu trong DangNhapController với action DangNhap
            if (Session["taikhoan"] == null )
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            //TTH1_end

            //TTH2_start
            //đây là cách truyền dữ liệu từ controller sang view , đoạn này load dữ liệu từ bảng tbl_sanpham va tbl_giohang với điều kiện 
            //id_user bên bảng tbl_giohang bằng với session["taikhoan"] và id_sanpham bên bàng tbl_sanpham bằng với id_sanpham bên bảng 
            //tbl_giohang , và sau khi chuyển dữ liệu sang dạng tolist vào biến lstgiohang , thì kiểm tra lstgiohang có giá trị hay không
            //nếu không có giá trị thì chuyển sang action Index của HomeController ngược lại trả về action GioHang của GioHangController

            int taikhoan = Int32.Parse(Session["taikhoan"].ToString());
            var lstgiohang = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              where sp.Id == gh.Id_MaSanPham && gh.Id_user == taikhoan && gh.TrangThai==1
                              select new doanweb.Models.tbl_sanpham
                              {
                                 Id= sp.Id,
                                 TenSanPham = sp.TenSanPham,
                                 NgayNhap = gh.NgayNhap,
                                 GiaTien = sp.GiaTien,
                                 ChiTietSanPham = sp.ChiTietSanPham,
                                 NhaSanXuat = sp.NhaSanXuat,
                                 LoaiSanPham = sp.LoaiSanPham,
                                 AnhSanPham = sp.AnhSanPham,
                                 //cái số lượng sản phẩm này của bàng tbl_giohang chứ không phải của bàng tbl_sanpham , nhưng sao lại 
                                 //chạy được là do có dòng TTH2_soluongsanpham_GioHangController bên class tbl_sanpham.cs trong Model1.tt 
                                 SoLuongSanPham=gh.SoLuongSanPham
                                 
                              }).ToList();
                    //TTH2.1_start
                    //chỗ này kiểm tra biến lstgiohang có giá trị hay không 
            if (!lstgiohang.Any())
            {
                return RedirectToAction("Index", "Home");
            }
                    //TTH2.1_end

            return View(lstgiohang);
            //TTH2_end
        }

        //Hàm này dùng để xử lý cho frmThemGioHang , khi dùng người dùng bấm vào nút them vào giỏ hàng thì frmThemGioHang sẽ truyền dữ 
        //liệu  gồm mã sản phẩm và url của trang , để sau khi người dùng thêm xog thì trả về trag đang thêm 
        public ActionResult ThemGioHang(int iMaSp=0,string url="")
        {
            //TTH1_start
            //kiểm tra xem sản phẩm từ mã nhập vào có tồn tại hay không nếu thì trả về action BaoLoiTrang của SanPhamController
            tbl_sanpham sp = db.tbl_sanpham.SingleOrDefault(n => n.Id == iMaSp);
            if(sp==null)
            {
                return RedirectToAction("BaoLoiTrang", "SanPham");
            }
            //TTH1_end

            //TTH2_start 
            //kiểm tra xem người dùng có đăng nhập chưa nếu chưa thì chuyển sang trang đăng nhập
            if (Session["taikhoan"] == null)
           {
               return RedirectToAction("DangNhap", "DangNhap");
           }
            //TTH2_end

            //TTH3_start
            //kiểm tra trong giỏ hàng của người dùng có sản phẩm này chưa , nếu chưa thì thêm vào còn có rồi thì tăng số lượng lên
            tbl_giohang giohang;
            int taikhoan = Int32.Parse(Session["taikhoan"].ToString());
            var lstgiohang=db.tbl_giohang.Where(n=>n.Id_user==taikhoan && n.Id_MaSanPham==iMaSp && n.TrangThai==1).ToList();
            if(lstgiohang.Count==0)
            {
                giohang = new tbl_giohang();
                giohang.Id_MaSanPham=iMaSp;
                giohang.Id_user=Int32.Parse(Session["taikhoan"].ToString());
                giohang.SoLuongSanPham=1;
                giohang.NgayNhap = DateTime.Now;
                giohang.TrangThai=1;
                db.tbl_giohang.Add(giohang);

                /////////////////////////////////////////////////////////////////
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == iMaSp);


                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = 1;
                moi.NgayNhap = DateTime.Now;
                moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Thêm Vào Giỏ Hàng Sản Phẩm Mới - Khách Hàng";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////

                db.SaveChanges();
                return Redirect(url);
            }
            else
            {
                giohang = db.tbl_giohang.FirstOrDefault(n => n.Id_user == taikhoan && n.Id_MaSanPham == iMaSp && n.TrangThai == 1);
                giohang.SoLuongSanPham = giohang.SoLuongSanPham+1;


                /////////////////////////////////////////////////////////////////
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == iMaSp);


                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = giohang.SoLuongSanPham ;
                moi.NgayNhap = DateTime.Now;
                moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Thêm Vào Giỏ Hàng Sản Phẩm Đã Có - Khách Hàng";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////


                db.SaveChanges();
                return Redirect(url);
            }
            //TTH3_end

        }

        //TTH4_start
        //xu lý xóa sản phẩm trong giỏ hàng
        public ActionResult XoaGioHang(int iMaSp=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            int taiKhoan=Int32.Parse(Session["taikhoan"].ToString());
            tbl_giohang gh = db.tbl_giohang.FirstOrDefault(n => n.Id_MaSanPham == iMaSp && n.Id_user == taiKhoan && n.TrangThai==1);
            if(gh!=null)
            {
                db.tbl_giohang.Remove(gh);


                /////////////////////////////////////////////////////////////////
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taiKhoan);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == iMaSp);


                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = gh.SoLuongSanPham;
                moi.NgayNhap = gh.NgayNhap;
                moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Xóa Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////

                db.SaveChanges();
            }
            return RedirectToAction("GioHang", "GioHang"); 
        }

        //TTH4_end
        
        //TTH5_start
        //xử lý tăng số lượng cùa 1 sản phẩm trong giỏ hàng
        public ActionResult TangGioHang(int iMaSp=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            int taiKhoan = Int32.Parse(Session["taikhoan"].ToString());
            tbl_giohang gh = db.tbl_giohang.FirstOrDefault(n => n.Id_MaSanPham == iMaSp && n.Id_user == taiKhoan && n.TrangThai==1);
            if (gh != null)
            {
                if(gh.SoLuongSanPham<10000)
                {
                    gh.SoLuongSanPham += 1;

                    /////////////////////////////////////////////////////////////////
                    tbl_Hgiohang moi = new tbl_Hgiohang();
                    var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taiKhoan);
                    var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == iMaSp);


                    moi.TenKhachHang = nglichsu.TenDangNhap;
                    moi.TenSanPham = splichsu.TenSanPham;
                    moi.SoLuongSanPham = gh.SoLuongSanPham;
                    moi.NgayNhap = gh.NgayNhap;
                    moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                    moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                    moi.TrangThaiThayDoi = "Tăng Số Lượng Sản Phẩm Trong Giỏ Hàng - Khách Hàng";
                    moi.NgayThayDoi = DateTime.Now;
                    db.tbl_Hgiohang.Add(moi);

                    //////////////////////////////////////////////////////////////

                    db.SaveChanges();
                }
            }
            return RedirectToAction("GioHang", "GioHang"); 
        }
        //TTH5_end

        //TTH6_start
        //xử lý giảm số lượng cùa 1 sản phẩm trong giỏ hàng
        public ActionResult GiamGioHang(int iMaSp = 0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            int taiKhoan = Int32.Parse(Session["taikhoan"].ToString());
            tbl_giohang gh = db.tbl_giohang.FirstOrDefault(n => n.Id_MaSanPham == iMaSp && n.Id_user == taiKhoan && n.TrangThai==1);
            if (gh != null)
            {
                if(gh.SoLuongSanPham>1)
                {
                    gh.SoLuongSanPham -= 1;


                    /////////////////////////////////////////////////////////////////
                    tbl_Hgiohang moi = new tbl_Hgiohang();
                    var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taiKhoan);
                    var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == iMaSp);


                    moi.TenKhachHang = nglichsu.TenDangNhap;
                    moi.TenSanPham = splichsu.TenSanPham;
                    moi.SoLuongSanPham = gh.SoLuongSanPham;
                    moi.NgayNhap = gh.NgayNhap;
                    moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                    moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                    moi.TrangThaiThayDoi = "Giảm Số Lượng Sản Phẩm Trong Giỏ Hàng - Khách Hàng";
                    moi.NgayThayDoi = DateTime.Now;
                    db.tbl_Hgiohang.Add(moi);

                    //////////////////////////////////////////////////////////////

                    db.SaveChanges();
                }
            }
            return RedirectToAction("GioHang", "GioHang");
        }
        //TTH5_end

        //TTH6_start
        public ActionResult ThanhToan()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            int taiKhoan = Int32.Parse(Session["taikhoan"].ToString());
            var lstspgh = db.tbl_giohang.Where(n => n.Id_user == taiKhoan && n.TrangThai==1).ToList();
            foreach(var item in lstspgh)
            {
                item.NgayNhap = DateTime.Now;
                item.TrangThai = 2;

                /////////////////////////////////////////////////////////////////
                tbl_Hgiohang moi = new tbl_Hgiohang();
                var nglichsu = db.tbl_member.FirstOrDefault(n => n.Id == taiKhoan);
                var splichsu = db.tbl_sanpham.FirstOrDefault(n => n.Id == item.Id_MaSanPham);


                moi.TenKhachHang = nglichsu.TenDangNhap;
                moi.TenSanPham = splichsu.TenSanPham;
                moi.SoLuongSanPham = item.SoLuongSanPham;
                moi.NgayNhap = item.NgayNhap;
                moi.TrangThai = "Sản Phẩm Đang Nằm Trong Giỏ Hàng - Khách Hàng";
                moi.TenNguoiThayDoi = nglichsu.TenDangNhap;
                moi.TrangThaiThayDoi = "Thanh Toán Sản Phẩm Trong Giỏ Hàng - Khách Hàng";
                moi.NgayThayDoi = DateTime.Now;
                db.tbl_Hgiohang.Add(moi);

                //////////////////////////////////////////////////////////////

            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        //TTH6_end
    }
}
