using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using System.Web.Security;
using System.Text.RegularExpressions;
using doanweb.PublicClass;

namespace doanweb.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();
        [HttpGet]
        public ActionResult ThongTinCaNhan()
        {
            if(Session["taikhoan"]==null)
            {
                return RedirectToAction("DangNhap","DangNhap");
            }

            int taikhoan = Int32.Parse(Session["taikhoan"].ToString());

            tbl_member lst = db.tbl_member.FirstOrDefault(n =>n.Id ==taikhoan);
            //if (lst.DiaChi == null)
            //{
            //    lst.DiaChi = "";
            //}
            //if(lst.SDT==null)
            //{
            //    lst.SDT = "";
            //}

            lst.MatKhau = null;

            ViewBag.tieude_ttcn = "";
            ViewBag.tenkhachhang_ttcn = "";
            ViewBag.ngaysinh_ttcn = "";
            ViewBag.tendangnhap_ttcn = "";
            ViewBag.matkhau_ttcn = "";
            ViewBag.email_ttcn = "";
            ViewBag.sdt_ttcn = "";
            ViewBag.diachi_ttcn = "";
            return View(lst);
        }
        [HttpPost]
        public ActionResult ThongTinCaNhan(tbl_member kh)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            int taikhoan = Int32.Parse(Session["taikhoan"].ToString());

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(kh.TenKhachHang.ToString());
            if (ketqua == true)
            {
                ViewBag.tenkhachhang_ttcn = null;
            }
            else
            {
                ViewBag.tenkhachhang_ttcn = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if (kh.NgaySinh==null)
            {
                ViewBag.ngaysinh_ttcn = null;
            }
            else
            {
                var ngaynow = DateTime.Today.Day.ToString();
                var ngaynhap = kh.NgaySinh.Value.Day.ToString();
                var thangnow = DateTime.Now.Month.ToString();
                var thangnhap = kh.NgaySinh.Value.Month.ToString();
                var namnow = DateTime.Now.Year.ToString();
                var namnhap = kh.NgaySinh.Value.Year.ToString();
                if (int.Parse(namnhap) > int.Parse(namnow))
                {
                    ViewBag.ngaysinh_ttcn = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                }
                else if (int.Parse(namnhap) == int.Parse(namnow))
                {
                    if (int.Parse(thangnhap) > int.Parse(thangnow))
                    {
                        ViewBag.ngaysinh_ttcn = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                    }
                    else if (int.Parse(thangnhap) == int.Parse(thangnow))
                    {
                        if (int.Parse(ngaynhap) >= int.Parse(ngaynow))
                        {
                            ViewBag.ngaysinh_ttcn = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                        }
                        else
                        {
                            ViewBag.ngaysinh_ttcn = null;
                        }
                    }
                    else
                    {
                        ViewBag.ngaysinh_ttcn = null;
                    }
                }
                else
                {
                    ViewBag.ngaysinh_ttcn = null;
                }
            }


            ketqua = ktc.hamcatchuoi(kh.TenDangNhap.ToString());
            if (ketqua == false)
            {
                ViewBag.tendangnhap_ttcn = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
            }
            else
            {

                ViewBag.tendangnhap_ttcn = null;
            }

            if(kh.MatKhau==null)
            {
                ViewBag.matkhau_ttcn = null;
            }
            else
            {
                ketqua = ktc.hamkiemtradomanhmatkhau(kh.MatKhau.ToString());
                if (ketqua == false)
                {
                    ViewBag.matkhau_ttcn = "(Mật Khẩu Không Đủ Mạnh,Mật Khẩu Phải Trên 8 Ký Tự,Có Ký 1 Tự Hoa,Thường,Số,Và Ký Tự Đặc Biệt Vd: NguyenVanA@&5674)";
                }
                else
                {
                    ViewBag.matkhau_ttcn = null;
                }
            }

            ketqua = Regex.IsMatch(kh.Email.ToString(), @"^[a-z][a-z0-9_\.]{5,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$");
            if (ketqua == false)
            {
                ViewBag.email_ttcn = "(Vui Lòng Nhập Đúng Định Dạng Email Vd: Google@gmail.com)";
            }
            else
            {
                ViewBag.email_ttcn = null;
            }

            if(kh.SDT==null)
            {
                ViewBag.sdt_ttcn = null;
            }
            else
            {
                string sdt = kh.SDT.ToString().Replace("  ", string.Empty);
                ketqua = Regex.IsMatch(sdt, @"\D");
                if (ketqua==true)
                {
                    ViewBag.sdt_ttcn = "(Kiểu Số Điện Thoại Không Đúng)";
                }
                else
                {
                    if (sdt.Length > 11 || sdt.Length < 10)
                    {
                        ViewBag.sdt_ttcn = "(Số Điện Thoại Không Quá 11 Số Và Không Ít Hơn 10 Số)";
                    }
                    else
                    {
                        ViewBag.sdt_ttcn = null;
                    }
                }
            }

            if(kh.DiaChi==null)
            {
                ViewBag.diachi_ttcn = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(kh.DiaChi.ToString());
                if (ketqua == false)
                {
                    ViewBag.diachi_ttcn = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
                else
                {
                    ViewBag.diachi_ttcn = null;
                }
            }


            

            if (ViewBag.tenkhachhang_ttcn != null || ViewBag.ngaysinh_ttcn != null || ViewBag.tendangnhap_ttcn != null || ViewBag.matkhau_ttcn != null || ViewBag.email_ttcn != null || ViewBag.sdt_ttcn != null || ViewBag.diachi_ttcn != null)
            {
                tbl_member lst = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);
                //if (lst.DiaChi == null)
                //{
                //    lst.DiaChi = "";
                //}
                //if (lst.SDT == null)
                //{
                //    lst.SDT = "";
                //}

                lst.MatKhau = null;
                return View(lst);
            }


            
            //đầu tiên là kiểm tra tên đăng nhập này có tồn tại với id khác không,có nghĩa là tài khoản khác có tên đăng nhập này,nếu không thì sẽ 
            //kiểm tra tiếp ,là tên đăng nhập này có tồn tại với id tài khoản này không,nếu không tồn tại thì có nghĩa là tài khoản này sửa  tên đăng nhập
            //ngược lại có tồn tại có nghĩa tài khoản này k sửa tên đăng nhập
            var kttk = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == kh.TenDangNhap && n.Id != taikhoan);
            if (kttk==null)
            {
                tbl_member thaydoi = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);
                if(kh.MatKhau!=null)
                {
                    string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(kh.MatKhau, "MD5");
                    thaydoi.MatKhau = mahoamatkhau;
                }

                if(kh.SDT!=null)
                {
                    string sdt = kh.SDT.ToString().Replace("  ", "");
                    thaydoi.SDT = sdt;
                }
                
                if(kh.NgaySinh!=null)
                {
                    thaydoi.NgaySinh = kh.NgaySinh;
                }

                if(kh.DiaChi!=null)
                {
                    thaydoi.DiaChi = kh.DiaChi;
                }
                thaydoi.Email = kh.Email;
                thaydoi.TenDangNhap = kh.TenDangNhap;
                thaydoi.TenKhachHang = kh.TenKhachHang;

                db.SaveChanges();
                ViewBag.tieude_ttcn = "(LƯU THÀNH CÔNG)";
                return View(thaydoi);
            }
            else
            {
                ViewBag.tieude_ttcn = "(TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU CỦA TÀI KHOẢN ĐÃ TỒN TẠI)";
                tbl_member lst = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);
                //if (lst.DiaChi == null)
                //{
                //    lst.DiaChi = "";
                //}
                //if (lst.SDT == null)
                //{
                //    lst.SDT = "";
                //}

                lst.MatKhau = null;
                return View(lst);
            }

            
        }

    }
}
