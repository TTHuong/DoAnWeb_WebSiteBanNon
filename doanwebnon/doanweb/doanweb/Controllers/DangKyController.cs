using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using System.Net;
//dung thu viện này để thêm JavaScriptSerializer
using System.Web.Script.Serialization;
//thêm thư mục publicClass de co thể sử dụng các lớp bên trong
using doanweb.PublicClass;
//thư viện dùng để sử dụng các hàm như regex
using System.Text.RegularExpressions;
//thư viện dùng để mã hóa mật khẩu
using System.Web.Security;
//thư viện của facebook dùng để gọi các hàm của facebook
using Facebook;
//thư viện này giúp thực hiện config
using System.Configuration;
//thư viện của google
using ASPSnippets.GoogleAPI;

namespace doanweb.Controllers
{
    public class DangKyController : Controller
    {
        //khởi tạo database được thêm từ model
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();

        KiemTraChuoiClass ktc = new KiemTraChuoiClass();
        //TTH1_start
        //hàm này xu lý cho form đăng ký (frmdangky) khi mới load trang web lên
        [HttpGet]
        public ActionResult Dangky()
        {
            if(Session["taikhoan"]!=null)
            {
                return RedirectToAction("ThongTinCaNhan", "ThongTinCaNhan");
            }
            ViewBag.tieudedk = "";
            ViewBag.tenkhachhangdk = "";
            ViewBag.ngaysinhdk = "";
            ViewBag.tendangnhapdk = "";
            ViewBag.matkhaudk = "";
            ViewBag.emaildk = "";
            ViewBag.sdtdk = "";
            ViewBag.diachidk = "";
            return View();
        }
        //TTH1_end

        //TTH2_start
        //hàm này xử lý form đăng ký (frmdangky) khi người dùng bấm button submit (sbdangky) , frmdangky sẽ trả về cho hàm 
        //1 bảng dữ liệu tbl_member 
        [HttpPost]
        public ActionResult Dangky(tbl_member kh)
        {
            //đặt cờ hiệu tên ketqua kiểu dữ liệu là bool và khởi tạo là true
            bool ketqua=true;
            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm cắt chuỗi , để xem người dùng có nhập ký tự đặc biệt hay không
            ketqua =ktc.hamcatchuoi(kh.TenKhachHang.ToString());

            if(ketqua==true)
            {
                ViewBag.tenkhachhangdk = null;
            }
            else
            {
                ViewBag.tenkhachhangdk = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            //TTH2.1_start
            //đoạn lệnh này sẽ kiểm tra người dùng có chọn ngày sình chưa,nếu chưa hiện thông báo ,ngược lại sẽ kiểm tra ngày sinh 
            //không được lớn hơn ngày hiện tại ,nếu lớn hơn sẽ có thông báo
            if(kh.NgaySinh==null)
            {
                ViewBag.ngaysinhdk = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
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
                    ViewBag.ngaysinhdk = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                }
                else if (int.Parse(namnhap) == int.Parse(namnow))
                {
                    if (int.Parse(thangnhap) > int.Parse(thangnow))
                    {
                        ViewBag.ngaysinhdk = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                    }
                    else if (int.Parse(thangnhap) == int.Parse(thangnow))
                    {
                        if (int.Parse(ngaynhap) >= int.Parse(ngaynow))
                        {
                            ViewBag.ngaysinhdk = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                        }
                        else
                        {
                            ViewBag.ngaysinhdk = null;
                        }
                    }
                    else
                    {
                        ViewBag.ngaysinhdk = null;
                    }
                }
                else
                {
                    ViewBag.ngaysinhdk = null;
                }
            }
            //TTH2.1_end


            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm cắt chuỗi , để xem người dùng có nhập ký tự đặc biệt hay không
            ketqua = ktc.hamcatchuoi(kh.TenDangNhap.ToString());
            if (ketqua == false)
            {
                ViewBag.tendangnhapdk = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
            }
            else
            {

                ViewBag.tendangnhapdk = null;
            }

            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm kiểm tra độ mạnh của mật khẩu , để xem người dùng có nhập mật khẩu đủ mạnh hay không
            ketqua = ktc.hamkiemtradomanhmatkhau(kh.MatKhau.ToString());
            if(ketqua==false)
            {
                ViewBag.matkhaudk = "(Mật Khẩu Không Đủ Mạnh,Mật Khẩu Phải Trên 8 Ký Tự,Có Ký 1 Tự Hoa,Thường,Số,Và Ký Tự Đặc Biệt Vd: NguyenVanA@&5674)";
            }
            else
            {
                ViewBag.matkhaudk = null;
            }

            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm Kiemtraemail , để xem người dùng có nhập đúng dạng email hay không
            ketqua = ktc.Kiemtraemail(kh.Email);
            if(ketqua==false)
            {
                ViewBag.emaildk = "(Vui Lòng Nhập Đúng Định Dạng Email Vd: Google@gmail.com)";
            }
            else
            {
                ViewBag.emaildk = null;
            }

            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm Kiemtrasdt , để xem người dùng có nhập đúng dạng số điện thoại hay không
            ketqua = ktc.Kiemtrasdt(kh.SDT.ToString());
            if(ketqua==true)
            {
                ViewBag.sdtdk = "(Kiểu Số Điện Thoại Không Đúng)";
            }
            else
            {
                if (kh.SDT.ToString().Length > 11 || kh.SDT.ToString().Length<10)
                {
                    ViewBag.sdtdk = "(Số Điện Thoại Không Quá 11 Số Và Không Ít Hơn 10 Số)";
                }
                else
                {
                    ViewBag.sdtdk = null;
                }
            }

            //cờ hiệu lúc này sẽ mang giá trị trả về của hàm cắt chuỗi , để xem người dùng có nhập ký tự đặc biệt hay không
            ketqua = ktc.hamcatchuoi(kh.DiaChi.ToString());
            if (ketqua == false)
            {
                ViewBag.diachidk = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }
            else
            {
                ViewBag.diachidk = null;
            }

            if (ViewBag.tenkhachhangdk != null || ViewBag.ngaysinhdk != null || ViewBag.tendangnhapdk != null || ViewBag.matkhaudk != null || ViewBag.emaildk != null || ViewBag.sdtdk != null || ViewBag.diachidk != null)
            {
                return View();
            }

            
            //biến taikhoan sẽ nhận giá trị từ database với điều kiện là tên đăng nhập trong database 
            //bằng với tên đăng nhập trong frmdangky
            string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(kh.MatKhau.ToString(), "MD5");
            var taikhoan=db.tbl_member.FirstOrDefault(n=>n.TenDangNhap==kh.TenDangNhap);
            //nếu biến taikhoan có giá trị có nghĩa là tồn tại tài khoản này thì tiến hành đăng nhập , ngược lại thì tiến hành đăng ký
            //tài khoản này
            if(taikhoan!=null)
            {
                if(taikhoan.MatKhau.CompareTo(mahoamatkhau)==0)
                {
                    ViewBag.tieudedk = "";
                    Session["taikhoan"] = taikhoan.Id;
                    Session["quyenhan"] = taikhoan.QuyenHan;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.tieudedk = "(THÔNG TIN TÀI KHOẢN ĐÃ TỒN TẠI)";
                    return View();
                }
                
            }
            


            kh.XacMinh = null;
            kh.MatKhau = mahoamatkhau;
            kh.QuyenHan = 1;
            db.tbl_member.Add(kh);

            ///////////////////////////////////////////////////////////////////////////////////
            tbl_Hmember moi = new tbl_Hmember();
            moi.TenDangNhap = kh.TenDangNhap;
            moi.TenDangNhap_Cu = kh.TenDangNhap;
            moi.MatKhau = kh.MatKhau;
            moi.MatKhau_Cu = kh.MatKhau;
            moi.QuyenHan = "Khách Hàng";
            moi.QuyenHan_Cu = "Khách Hàng";
            moi.TenKhachHang = kh.TenKhachHang;
            moi.TenKhachHang_Cu = kh.TenKhachHang;
            moi.NgaySinh = kh.NgaySinh;
            moi.NgaySinh_Cu = kh.NgaySinh;
            moi.SDT = kh.SDT;
            moi.SDT_Cu = kh.SDT;
            moi.DiaChi = kh.DiaChi;
            moi.DiaChi_Cu = kh.DiaChi;
            moi.Email = kh.Email;
            moi.Email_Cu = kh.Email;
            moi.NguoiThayDoi = kh.TenDangNhap;
            moi.NgayThayDoi = DateTime.Now;
            moi.TrangThai = "Thêm Tài Khoản Người Dùng - Khách Hàng";
            db.tbl_Hmember.Add(moi);

            //////////////////////////////////////////////////////////////////////////////////


            db.SaveChanges();
            return RedirectToAction("DangNhap", "DangNhap");
        }
        //TTH2_end

        //TTH3_start
        //khi bấm vào thẻ a facebook trong view DangKy.cshtml nó sẽ chạy hàm DangNhapFacebook() 
        public ActionResult DangKyFacebook()
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
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.id;
                string tenkhachhang = me.first_name + " " + me.last_name;
                string matkhau = me.id;
                string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(matkhau, "MD5");

                var timkiemtaikhoan = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == username && n.MatKhau==mahoamatkhau );

                if (timkiemtaikhoan!=null)
                {
                    ViewBag.tieudedk = "";
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
                    user.XacMinh = null;

                    db.tbl_member.Add(user);


                    ///////////////////////////////////////////////////////////////////////////////////
                    tbl_Hmember moi = new tbl_Hmember();
                    moi.TenDangNhap = username;
                    moi.TenDangNhap_Cu = username;
                    moi.MatKhau = mahoamatkhau;
                    moi.MatKhau_Cu = mahoamatkhau;
                    moi.QuyenHan = "Khách Hàng";
                    moi.QuyenHan_Cu = "Khách Hàng";
                    moi.TenKhachHang = tenkhachhang;
                    moi.TenKhachHang_Cu = tenkhachhang;
                    moi.NgaySinh = null;
                    moi.NgaySinh_Cu = null;
                    moi.SDT = null;
                    moi.SDT_Cu = null;
                    moi.DiaChi = null;
                    moi.DiaChi_Cu = null;
                    moi.Email = email;
                    moi.Email_Cu = email;
                    moi.NguoiThayDoi = username;
                    moi.NgayThayDoi = DateTime.Now;
                    moi.TrangThai = "Thêm Tài Khoản Người Dùng - Khách Hàng";
                    db.tbl_Hmember.Add(moi);

                    //////////////////////////////////////////////////////////////////////////////////


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

        //TTH4_start
        public class GoogleProfile
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public Image Image { get; set; }
            public List<Email> Emails { get; set; }
            public string Gender { get; set; }
            public string ObjectType { get; set; }
        }

        public class Email
        {
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class Image
        {
            public string Url { get; set; }
        }
        //public void DangKyGoogle()
        //{
             
        //    //GgAppId, GgAppSecret lấy từ Web.config 
        //    //2 dòng đầu gán giá trị Client ID ( GgAppId ) và Client Secret ( GgAppSecret ) qua 2 thuộc tính ClientID và ClientSecret 
        //    //của class GoogleConnect
        //    GoogleConnect.ClientId = ConfigurationManager.AppSettings["GgAppId"];
        //    GoogleConnect.ClientSecret = ConfigurationManager.AppSettings["GgAppSecret"];
        //    GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
        //    GoogleConnect.Authorize("profile", "email");
        //}
        public ActionResult DangKyGoogle()
        {
            GoogleConnect.ClientId = ConfigurationManager.AppSettings["GgAppId"];
            GoogleConnect.ClientSecret = ConfigurationManager.AppSettings["GgAppSecret"];
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("profile", "email");

            if(!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                GoogleProfile profile = new
                JavaScriptSerializer().Deserialize<GoogleProfile>(json);

                string username=profile.Id;
                
                string tenkhachhang=profile.DisplayName;
                WebSiteBanNonEntities db = new WebSiteBanNonEntities();
                //var lstAccount=db.tbl_member.Where(n=>n.)
                
            }
            return RedirectToAction("Index", "Home");
        }
        //TTH4_end
    }
}
