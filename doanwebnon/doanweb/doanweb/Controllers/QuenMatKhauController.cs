using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.PublicClass;
using doanweb.Models;
using System.Configuration;
using System.Web.Security;

namespace doanweb.Controllers
{
    public class QuenMatKhauController : Controller
    {
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        [HttpGet]
        public ActionResult QuenMatKhau()
        {
            ViewBag.tieude_llmk = "";
            ViewBag.tendangnhap_llmk = "";
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection f)
        {
            bool ketqua = true;
            ////cờ hiệu lúc này sẽ mang giá trị giả về của hàm cắt chuỗi , để xem người dùng có nhập ký tự đặc biệt hay không
            ketqua = ktc.hamcatchuoi(f.Get("txtquenmatkhau").ToString());
            if (ketqua == false)
            {
                ViewBag.tendangnhap_llmk = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
                return View();
            }
            else
            {
                ViewBag.tendangnhap_llmk = null;
            }
            string tendangnhap = f.Get("txtquenmatkhau");
            tbl_member tk = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == tendangnhap);
            if(tk==null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                SendEmailClass.GmailUsername = ConfigurationManager.AppSettings["FromEmailAddress"];
                SendEmailClass.GmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"];

                SendEmailClass mailer = new SendEmailClass();
                mailer.ToEmail = tk.Email;
                mailer.Subject = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string body = System.IO.File.ReadAllText(Server.MapPath("~/PublicHtml/SendEmail.html"));
                body = body.Replace("{{Email}}", tk.Email);
                body = body.Replace("{{TenKhachHang}}", tk.TenKhachHang);
                body = body.Replace("{{TenDangNhap}}", tk.TenDangNhap);
                body = body.Replace("{{NgayGui}}", DateTime.Now.ToString());

                Random rd = new Random();
                bool kq=false;
                string id="";
                while(kq!=true)
                {
                     id= rd.Next(1, 999999999).ToString();
                    tbl_member temp = db.tbl_member.FirstOrDefault(n => n.XacMinh == id);
                    if(temp==null)
                    {
                        kq = true;
                        break;
                    }
                }

                body = body.Replace("{{Link}}", ConfigurationManager.AppSettings["XacNhanMaTuGmail"] + id);
                mailer.Body = body;
                mailer.IsHtml = true;
                mailer.Send();

                tk.XacMinh = id;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult EmailResetPassword(string MaXacNhan="")
        {
            tbl_member tk = db.tbl_member.FirstOrDefault(n => n.XacMinh == MaXacNhan);
            if(tk==null)
            {
                return RedirectToAction("QuenMatKhau", "QuenMatKhau");
            }
            else
            {
                Session["MaXacNhan"] = MaXacNhan;
                return View();
            }
        }
        [HttpPost]
        public ActionResult EmailResetPassword(FormCollection f)
        {
            bool ketqua = true;
            ketqua = ktc.hamkiemtradomanhmatkhau(f.Get("txtmatkhaumoi").ToString());
            if (ketqua == false)
            {
                ViewBag.matkhau_erp = "(Mật Khẩu Không Đủ Mạnh,Mật Khẩu Phải Trên 8 Ký Tự,Có Ký 1 Tự Hoa,Thường,Số,Và Ký Tự Đặc Biệt Vd: NguyenVanA@&5674)";
                return View();
            }
            else
            {
                ViewBag.matkhau_erp = null;
            }

            if(Session["MaXacNhan"]==null)
            {
                ViewBag.tieude_erp="(GẶP SỰ CỐ KHI ĐẶT LẠI MẬT KHẨU ,VUI LÒNG CLICK VÀO LINK Ở TRONG GMAIL LẠI !)";
                return RedirectToAction("QuenMatKhau", "QuenMatKhau");
            }
            else
            {
                string maxacnhan=Session["MaXacNhan"].ToString();
                tbl_member tk = db.tbl_member.FirstOrDefault(n => n.XacMinh ==maxacnhan );
                if (tk == null)
                {
                    return RedirectToAction("QuenMatKhau", "QuenMatKhau");
                }
                else
                {
                    tk.XacMinh = null;
                    string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(f.Get("txtmatkhaumoi").ToString(), "MD5");
                    

                    ///////////////////////////////////////////////////////////////////////////////////
                    tbl_Hmember moi = new tbl_Hmember();
                    var nglichsu = db.tbl_member.FirstOrDefault(n => n.XacMinh == maxacnhan);

                    moi.TenDangNhap = nglichsu.TenDangNhap;
                    moi.TenDangNhap_Cu = nglichsu.TenDangNhap;
                    moi.MatKhau = mahoamatkhau;
                    moi.MatKhau_Cu = nglichsu.MatKhau;
                    moi.QuyenHan = "Khách Hàng";
                    moi.QuyenHan_Cu = "Khách Hàng";
                    moi.TenKhachHang = nglichsu.TenKhachHang;
                    moi.TenKhachHang_Cu = nglichsu.TenKhachHang;
                    moi.NgaySinh = nglichsu.NgaySinh;
                    moi.NgaySinh_Cu = nglichsu.NgaySinh;
                    moi.SDT = nglichsu.SDT;
                    moi.SDT_Cu = nglichsu.SDT;
                    moi.DiaChi = nglichsu.DiaChi;
                    moi.DiaChi_Cu = nglichsu.DiaChi;
                    moi.Email = nglichsu.Email;
                    moi.Email_Cu = nglichsu.Email;
                    moi.NguoiThayDoi = nglichsu.TenDangNhap;
                    moi.NgayThayDoi = DateTime.Now;
                    moi.TrangThai = "Thêm Tài Khoản Người Dùng - Khách Hàng";
                    db.tbl_Hmember.Add(moi);

                    //////////////////////////////////////////////////////////////////////////////////

                    tk.MatKhau = mahoamatkhau;
                    db.SaveChanges();
                    Session.Remove("MaXacNhan");
                    return RedirectToAction("DangNhap", "DangNhap");
                }
            }
        }
    }
}
