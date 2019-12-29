using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
//thư viện này giúp thực hiện config
using System.Configuration;
//thư viện dùng để sử dụng các hàm như regex
using System.Text.RegularExpressions;
//thư viện dùng để mã hóa mật khẩu
using System.Web.Security;
//thư viện của facebook dùng để gọi các hàm của facebook
using Facebook;

using doanweb.PublicClass;

namespace doanweb.Controllers
{
    public class DangNhapController : Controller
    {
        //khởi tạo database được thêm từ model
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();
        
        //TTH1_start
        //hàm này xu lý cho form đăng nhập (frmdangnhap) khi mới load trang web lên
        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["taikhoan"] != null)
            {
                return RedirectToAction("ThongTinCaNhan", "ThongTinCaNhan");
            }
            ViewBag.tieude_dn = "";
            ViewBag.tendangnhap_dn = "";
            ViewBag.matkhau_dn = "";
            return View();
        }
        //TTH1_end

        //TTH2_start
        //hàm này xử lý form đăng nhập (frmdangnhap) khi người dùng bấm button submit (sbdangnhap) , frmdangnhap sẽ trả về cho hàm 
        //1 bảng dữ liệu tbl_member 
        [HttpPost]
        public ActionResult DangNhap(tbl_member kh)
        {
            //đặt cờ hiệu tên ketqua kiểu dữ liệu là bool và khởi tạo là true
            bool ketqua = true;
            //cờ hiệu lúc này sẽ mang giá trị giả về của hàm cắt chuỗi , để xem người dùng có nhập ký tự đặc biệt hay không
            ketqua = ktc.hamcatchuoi(kh.TenDangNhap.ToString());
            if (ketqua == false)
            {
                ViewBag.tendangnhap_dn = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
                return View();
            }
            else
            {
                ViewBag.tendangnhap_dn = null;
            }
            //biến timkiemtaikhoan sẽ nhận giá trị từ database với điều kiện là tên đăng nhập trong database 
            //bằng với tên đăng nhập trong frmdangnhap và giá trị của biến mahoamatkhau bằng với mật khẩu trong database 
            string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(kh.MatKhau.ToString(), "MD5");
            var timkiemtaikhoan = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == kh.TenDangNhap && n.MatKhau==mahoamatkhau);
            //nếu biến timkiemtaikhoan có giá trị có nghĩa là tồn tại tài khoản này thì tiến hành đăng nhập
            // ngược lại thì trả về thông báo tài khoản không đúng
            if(timkiemtaikhoan!=null)
            {

                ViewBag.tieude_dn = "";
                Session["taikhoan"] = timkiemtaikhoan.Id;
                Session["quyenhan"] = timkiemtaikhoan.QuyenHan;
                if(Int32.Parse(Session["quyenhan"].ToString())==1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if(Int32.Parse(Session["quyenhan"].ToString())==2)
                {
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            else
            {
                ViewBag.tieude_dn = "(TÀI KHOẢN VÀ MẬT KHẨU KHÔNG ĐÚNG)";
                return View();
            }
            return View();
        }
        //TTH2_end

        //TTH3_start
        //khi bấm vào thẻ a facebook trong view DangNhap.cshtml nó sẽ chạy hàm DangNhapFacebook() 
        public ActionResult DangNhapFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                //FbAppId , FbAppSecret nằm trong Web.config , nó là id và mật khẩu của ứng dụng facebook do mình tạo ra
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token",new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if(!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=id,name,email");
                string email = me.email;
                string username = me.id;
                string tenkhachhang = me.name;
                string matkhau = me.id.ToString();

                string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(matkhau, "MD5");
                var timkiemtaikhoan = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == username && n.MatKhau==mahoamatkhau );
                
                if (timkiemtaikhoan!=null)
                {
                    ViewBag.tieude_dn = "";
                    Session["taikhoan"] = timkiemtaikhoan.Id;
                    Session["quyenhan"] = timkiemtaikhoan.QuyenHan;
                    if (Int32.Parse(Session["quyenhan"].ToString()) == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (Int32.Parse(Session["quyenhan"].ToString()) == 2)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    
                }
                else
                {
                    tbl_member user = new tbl_member();
                    user.Email = email;
                    user.TenDangNhap = username;
                    user.TenKhachHang = tenkhachhang;
                    user.MatKhau = mahoamatkhau;
                    user.QuyenHan = 1;

                    db.tbl_member.Add(user);
                    db.SaveChanges();

                    tbl_member iduser = db.tbl_member.SingleOrDefault(n => n.TenDangNhap == username && n.MatKhau == mahoamatkhau);
                    Session["taikhoan"] = iduser.Id;
                    Session["quyenhan"] = iduser.QuyenHan;
                    if (Int32.Parse(Session["quyenhan"].ToString()) == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (Int32.Parse(Session["quyenhan"].ToString()) == 2)
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }
        //TTH3_end



    }
}
