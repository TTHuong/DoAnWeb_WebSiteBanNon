using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using doanweb.PublicClass;
using System.Text.RegularExpressions;
using System.IO;

namespace doanweb.Controllers
{
    public class Admin_QLSPController : Controller
    {

        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();

        public ActionResult QuanLySanPham()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstsp = (from sp in db.tbl_sanpham.AsEnumerable()
                        from nsx in db.tbl_nhasanxuat.AsEnumerable()
                        where sp.NhaSanXuat==nsx.Id
                         select new doanweb.Models.tbl_sanpham
                         {
                             Id=sp.Id,
                             TenSanPham=sp.TenSanPham,
                             GiaTien=sp.GiaTien,
                             ChiTietSanPham=sp.ChiTietSanPham,
                             Tennhasanxuat=nsx.TenNhaSanXuat,
                             LoaiSanPham=sp.LoaiSanPham,
                             AnhSanPham=sp.AnhSanPham

                         }
                             ).ToList();
            return View(lstsp);
        }

        [HttpGet]
        public ActionResult QuanLySanPham_Them()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            //// Lấy data
            //// Lấy toàn bộ thể loại:
            //List<Category> cate = dbCate.Categories.ToList();

            //// Tạo SelectList
            //SelectList cateList = new SelectList(cate, "ID", "THELOAI_NAME");

            //// Set vào ViewBag
            //ViewBag.CategoryList = cateList;

            ViewBag.QLSP_T_TieuDe = null;
            ViewBag.QLSP_T_TenSanPham = null;
            ViewBag.QLSP_T_GiaTien = null;
            ViewBag.QLSP_T_ChiTietSanPham = null;
            ViewBag.QLSP_T_NhaSanXuat = null;
            ViewBag.QLSP_T_LoaiSanPham = null;
            ViewBag.QLSP_T_AnhSanPham = null;

            ViewBag.QLSP_T_AnhChiTiet1 = null;
            ViewBag.QLSP_T_AnhChiTiet2 = null;
            ViewBag.QLSP_T_AnhChiTiet3 = null;
            ViewBag.QLSP_T_AnhChiTiet4 = null;
            ViewBag.QLSP_T_AnhChiTiet5 = null;

            List<tbl_nhasanxuat> nsx = db.tbl_nhasanxuat.ToList();
            SelectList lstnsx = new SelectList(nsx, "Id", "TenNhaSanXuat");
            ViewBag.ListNhaSanXuat = lstnsx;
            return View();
        }

        [HttpPost]
        public ActionResult QuanLySanPham_Them(tbl_sanpham f, FormCollection fr, HttpPostedFileBase AnhSanPham)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            List<tbl_nhasanxuat> nsx = db.tbl_nhasanxuat.ToList();
            SelectList lstnsx = new SelectList(nsx, "Id", "TenNhaSanXuat");
            ViewBag.ListNhaSanXuat = lstnsx;

            if(f.TenSanPham==null)
            {
                ViewBag.QLSP_T_TenSanPham = "( Tên Sản Phẩm Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLSP_T_TenSanPham = null;
            }

            if (fr.Get("GiaTien") == null)
            {
                ViewBag.QLSP_T_GiaTien = "( Giá Tiền Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLSP_T_GiaTien = null;
            }

            if(f.ChiTietSanPham==null)
            {
                ViewBag.QLSP_T_ChiTietSanPham = "( Chi Tiết Sản Phẩm Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLSP_T_ChiTietSanPham = null;
            }

            if(f.NhaSanXuat==null)
            {
                ViewBag.QLSP_T_NhaSanXuat = "( Nhà Sản Xuất Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLSP_T_NhaSanXuat = null;
            }

            if(fr.Get("LoaiSanPham")==null)
            {
                ViewBag.QLSP_T_LoaiSanPham = "( Loại Sản Phẩm Không Được Bỏ Trống )";
            }
            else
            {
                if (fr.Get("LoaiSanPham").ToString() != "Nón Nam" && fr.Get("LoaiSanPham").ToString()!="Nón Nữ" && fr.Get("LoaiSanPham").ToString()!="Nón Cặp")
                {
                    ViewBag.QLSP_T_LoaiSanPham = "( Xảy Ra Lỗi Này Là Do Bạn Cố Thay Đổi Code Html ! Vui Lòng Load Lại Trang )";
                }
                else
                {
                    ViewBag.QLSP_T_LoaiSanPham = null;
                }
                
            }

            if (AnhSanPham == null || AnhSanPham.ContentLength == 0)
            {
                ViewBag.QLSP_T_AnhSanPham = "( Ảnh Sản Phẩm Không Được Bỏ Trống )";
            }
            else{
                ViewBag.QLSP_T_AnhSanPham = null;
            }

            if (ViewBag.QLSP_T_TenSanPham != null || ViewBag.QLSP_T_GiaTien != null || ViewBag.QLSP_T_ChiTietSanPham != null || ViewBag.QLSP_T_NhaSanXuat != null || ViewBag.QLSP_T_LoaiSanPham != null || ViewBag.QLSP_T_AnhSanPham != null)
            {
                return View();
            }

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(f.TenSanPham.ToString());
            if(ketqua==true)
            {
                ViewBag.QLSP_T_TenSanPham = null;
            }
            else
            {
                ViewBag.QLSP_T_TenSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            ketqua = ktc.Kiemtradangso(f.GiaTien.ToString());
            if(ketqua==true)
            {
                ViewBag.QLSP_T_GiaTien = null;
            }
            else
            {
                ViewBag.QLSP_T_GiaTien = "( Vui Lòng Nhập Đúng Dạng Số Tiền )";
            }

            ketqua = ktc.hamcatchuoi(f.ChiTietSanPham);
            if(ketqua==true)
            {
                ViewBag.QLSP_T_ChiTietSanPham = null;
            }
            else
            {
                ViewBag.QLSP_T_ChiTietSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            ketqua = ktc.hamcatchuoi(f.NhaSanXuat.ToString());
            if (ketqua == true)
            {
                ViewBag.QLSP_T_NhaSanXuat = null;
            }
            else
            {
                ViewBag.QLSP_T_NhaSanXuat =  "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            ketqua = ktc.hamcatchuoi(f.LoaiSanPham.ToString());
            if (ketqua == true)
            {
                ViewBag.QLSP_T_LoaiSanPham = null;
            }
            else
            {
                ViewBag.QLSP_T_LoaiSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if(AnhSanPham.ContentLength>104857600)
            {
                ViewBag.QLSP_T_AnhSanPham = "( Kích Thước Ảnh Tối Đa 100MB )";
            }
            else
            {
                ViewBag.QLSP_T_AnhSanPham = null;
            }

            
            if (ViewBag.QLSP_T_TenSanPham != null || ViewBag.QLSP_T_GiaTien != null || ViewBag.QLSP_T_ChiTietSanPham != null || ViewBag.QLSP_T_NhaSanXuat != null || ViewBag.QLSP_T_LoaiSanPham != null || ViewBag.QLSP_T_AnhSanPham != null )
            {
                return View();
            }
            int Idsp = 0;
            var spmn = db.tbl_sanpham.OrderByDescending(n => n.Id).FirstOrDefault();
            if(spmn!=null)
            {
                Idsp = spmn.Id;
            }
            string path = Server.MapPath("~/Content/images/sanpham/");
            string ex = Path.GetExtension(AnhSanPham.FileName);
            string namefile = path + "sp" + (Idsp + 1).ToString() + ex;
            if(!System.IO.File.Exists(namefile))
            {
                AnhSanPham.SaveAs(namefile);
            }
            f.AnhSanPham = "sp" + (Idsp + 1).ToString() + ex;
            db.tbl_sanpham.Add(f);
            db.SaveChanges();
            ViewBag.QLSP_T_TieuDe = "( THÊM SẢN PHẨM THÀNH CÔNG )";
            return View();
        }

        public ActionResult QuanLySanPham_Xoa(int MaXoa=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var sp = db.tbl_sanpham.FirstOrDefault(n => n.Id == MaXoa);
            if(sp!=null)
            {
                string path = Server.MapPath("~/Content/images/sanpham/"+sp.AnhSanPham);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                var lstgh = db.tbl_giohang.Where(n => n.Id_MaSanPham == MaXoa && (n.TrangThai == 1 || n.TrangThai == 2)).ToList();
                if(lstgh.Count!=0)
                {
                    foreach(var item in lstgh)
                    {
                        db.tbl_giohang.Remove(item);
                    }
                }

                var lstacsp = db.tbl_anhchitietsanpham.Where(n => n.Id_Sp == MaXoa).ToList();
                if (lstacsp.Count != 0)
                {
                    foreach (var item in lstacsp)
                    {
                        path = Server.MapPath("~/Content/images/chitietsanpham/" + item.DuongDan);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        db.tbl_anhchitietsanpham.Remove(item);
                    }
                }


                db.tbl_sanpham.Remove(sp);
                db.SaveChanges();
            }

            return RedirectToAction("QuanLySanPham", "Admin_QLSP");
        }

        [HttpGet]
        public ActionResult QuanLySanPham_Sua(int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.QLSP_S_TieuDe = null;
            ViewBag.QLSP_S_TenSanPham = null;
            ViewBag.QLSP_S_GiaTien = null;
            ViewBag.QLSP_S_ChiTietSanPham = null;
            ViewBag.QLSP_S_NhaSanXuat = null;
            ViewBag.QLSP_S_LoaiSanPham = null;
            ViewBag.QLSP_S_AnhSanPham = null;

            List<tbl_nhasanxuat> nsx = db.tbl_nhasanxuat.ToList();
            SelectList lstnsx = new SelectList(nsx, "Id", "TenNhaSanXuat");
            ViewBag.ListNhaSanXuat = lstnsx;

            var sps = (from sp in db.tbl_sanpham.AsEnumerable()
                      from Nsx in db.tbl_nhasanxuat.AsEnumerable()
                      where sp.NhaSanXuat == Nsx.Id  && sp.Id==MaSua
                      select new doanweb.Models.tbl_sanpham
                      {
                          Id = sp.Id,
                          TenSanPham = sp.TenSanPham,
                          GiaTien = sp.GiaTien,
                          ChiTietSanPham = sp.ChiTietSanPham,
                          Tennhasanxuat = Nsx.TenNhaSanXuat,
                          LoaiSanPham = sp.LoaiSanPham,
                          AnhSanPham = sp.AnhSanPham
                      }).FirstOrDefault();


            return View(sps);
        }
        [HttpPost]
        public ActionResult QuanLySanPham_Sua(tbl_sanpham f,FormCollection fr, HttpPostedFileBase AnhSanPham,int MaSua=0 )
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            List<tbl_nhasanxuat> nsx = db.tbl_nhasanxuat.ToList();
            SelectList lstnsx = new SelectList(nsx, "Id", "TenNhaSanXuat");
            ViewBag.ListNhaSanXuat = lstnsx;

            //if (f.TenSanPham == null)
            //{
            //    ViewBag.QLSP_S_TenSanPham = "( Tên Sản Phẩm Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_TenSanPham = null;
            //}

            //if (f.GiaTien == null)
            //{
            //    ViewBag.QLSP_S_GiaTien = "( Giá Tiền Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_GiaTien = null;
            //}

            //if (f.ChiTietSanPham == null)
            //{
            //    ViewBag.QLSP_S_ChiTietSanPham = "( Chi Tiết Sản Phẩm Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_ChiTietSanPham = null;
            //}

            //if (f.NhaSanXuat == null)
            //{
            //    ViewBag.QLSP_S_NhaSanXuat = "( Nhà Sản Xuất Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_NhaSanXuat = null;
            //}

            //if (f.LoaiSanPham == null)
            //{
            //    ViewBag.QLSP_S_LoaiSanPham = "( Loại Sản Phẩm Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_LoaiSanPham = null;
            //}

            //if (AnhSanPham == null || AnhSanPham.ContentLength == 0)
            //{
            //    ViewBag.QLSP_S_AnhSanPham = "( Ảnh Sản Phẩm Không Được Bỏ Trống )";
            //}
            //else
            //{
            //    ViewBag.QLSP_S_AnhSanPham = null;
            //}

            //if (ViewBag.QLSP_S_TenSanPham != null || ViewBag.QLSP_S_GiaTien != null || ViewBag.QLSP_S_ChiTietSanPham != null || ViewBag.QLSP_S_NhaSanXuat != null || ViewBag.QLSP_S_LoaiSanPham != null )
            //{
            //    var sps = (from sp in db.tbl_sanpham.AsEnumerable()
            //               from Nsx in db.tbl_nhasanxuat.AsEnumerable()
            //               where sp.NhaSanXuat == Nsx.Id && sp.Id == MaSua
            //               select new doanweb.Models.tbl_sanpham
            //               {
            //                   Id = sp.Id,
            //                   TenSanPham = sp.TenSanPham,
            //                   GiaTien = sp.GiaTien,
            //                   ChiTietSanPham = sp.ChiTietSanPham,
            //                   Tennhasanxuat = Nsx.TenNhaSanXuat,
            //                   LoaiSanPham = sp.LoaiSanPham,
            //                   AnhSanPham = sp.AnhSanPham
            //               }).FirstOrDefault();
            //    return View(sps);
            //}

            bool ketqua = true;
            if(f.TenSanPham==null)
            {
                ViewBag.QLSP_S_TenSanPham = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(f.TenSanPham);
                if (ketqua == true)
                {
                    ViewBag.QLSP_S_TenSanPham = null;
                }
                else
                {
                    ViewBag.QLSP_S_TenSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            if(fr.Get("GiaTien")=="")
            {
                ViewBag.QLSP_S_GiaTien = null;
            }
            else
            {
                ketqua = ktc.Kiemtradangso(f.GiaTien.ToString());
                if (ketqua == true)
                {
                    ViewBag.QLSP_S_GiaTien = null;
                }
                else
                {
                    ViewBag.QLSP_S_GiaTien = "( Vui Lòng Nhập Đúng Dạng Số Tiền )";
                }
            }
            
            if(f.ChiTietSanPham==null)
            {
                ViewBag.QLSP_S_ChiTietSanPham = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(f.ChiTietSanPham);
                if (ketqua == true)
                {
                    ViewBag.QLSP_S_ChiTietSanPham = null;
                }
                else
                {
                    ViewBag.QLSP_S_ChiTietSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }
            
            if(f.NhaSanXuat==null)
            {
                ViewBag.QLSP_S_NhaSanXuat = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(f.NhaSanXuat.ToString());
                if (ketqua == true)
                {
                    ViewBag.QLSP_S_NhaSanXuat = null;
                }
                else
                {
                    ViewBag.QLSP_S_NhaSanXuat = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
            }


            if(f.LoaiSanPham==null)
            {
                ViewBag.QLSP_S_LoaiSanPham = null;
            }
            else
            {
                if (fr.Get("LoaiSanPham").ToString() != "Nón Nam" && fr.Get("LoaiSanPham").ToString() != "Nón Nữ" && fr.Get("LoaiSanPham").ToString() != "Nón Cặp")
                {
                    ViewBag.QLSP_S_LoaiSanPham = "( Xảy Ra Lỗi Này Là Do Bạn Cố Thay Đổi Code Html ! Vui Lòng Load Lại Trang )";
                }
                else
                {
                    ketqua = ktc.hamcatchuoi(f.LoaiSanPham);
                    if (ketqua == true)
                    {
                        ViewBag.QLSP_S_LoaiSanPham = null;
                    }
                    else
                    {
                        ViewBag.QLSP_S_LoaiSanPham = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                    }
                }
                
            }
            

            if(AnhSanPham!=null && AnhSanPham.ContentLength >0)
            {
                if (AnhSanPham.ContentLength > 104857600)
                {
                    ViewBag.QLSP_S_AnhSanPham = "( Kích Thước Ảnh Tối Đa 100MB )";
                }
                else
                {
                    ViewBag.QLSP_S_AnhSanPham = null;
                }
            }
            

            if (ViewBag.QLSP_S_TenSanPham != null || ViewBag.QLSP_S_GiaTien != null || ViewBag.QLSP_S_ChiTietSanPham != null || ViewBag.QLSP_S_NhaSanXuat != null || ViewBag.QLSP_S_LoaiSanPham != null || ViewBag.QLSP_S_AnhSanPham != null)
            {
                var sps = (from sp in db.tbl_sanpham.AsEnumerable()
                           from Nsx in db.tbl_nhasanxuat.AsEnumerable()
                           where sp.NhaSanXuat == Nsx.Id && sp.Id == MaSua
                           select new doanweb.Models.tbl_sanpham
                           {
                               Id = sp.Id,
                               TenSanPham = sp.TenSanPham,
                               GiaTien = sp.GiaTien,
                               ChiTietSanPham = sp.ChiTietSanPham,
                               Tennhasanxuat = Nsx.TenNhaSanXuat,
                               LoaiSanPham = sp.LoaiSanPham,
                               AnhSanPham = sp.AnhSanPham
                           }).FirstOrDefault();
                ViewBag.QLSP_S_TieuDe = null;
                return View(sps);
            }

            var sua = db.tbl_sanpham.FirstOrDefault(n => n.Id == MaSua);
            if(sua!=null)
            {
                if(AnhSanPham!=null && AnhSanPham.ContentLength >0)
                {
                    string path = Server.MapPath("~/Content/images/sanpham/");
                    if (System.IO.File.Exists(path + sua.AnhSanPham))
                    {
                        System.IO.File.Delete(path + sua.AnhSanPham);
                    }
                    string ex = Path.GetExtension(AnhSanPham.FileName);
                    string namefile = path + "sp" + (MaSua).ToString() + ex;
                    AnhSanPham.SaveAs(namefile);

                    sua.AnhSanPham = "sp" + (MaSua).ToString() + ex;
                }
                
                if(f.TenSanPham!=null)
                {
                    sua.TenSanPham = f.TenSanPham;
                }
                if(f.GiaTien!=null)
                {
                    sua.GiaTien = f.GiaTien;
                }
                if(f.ChiTietSanPham!=null)
                {
                    sua.ChiTietSanPham = f.ChiTietSanPham;
                }
                if(f.NhaSanXuat!=null)
                {
                    sua.NhaSanXuat = f.NhaSanXuat;
                }
                if(f.LoaiSanPham!=null)
                {
                    sua.LoaiSanPham = f.LoaiSanPham;
                }
                
                db.SaveChanges();
                ViewBag.QLSP_S_TieuDe = "( CẬP NHẬT SẢN PHẨM THÀNH CÔNG )";
                return View(sua);

            }
            else
            {
                ViewBag.QLSP_S_TieuDe = "( CẬP NHẬT SẢN PHẨM THẤT BẠI )";
                var sps = (from sp in db.tbl_sanpham.AsEnumerable()
                           from Nsx in db.tbl_nhasanxuat.AsEnumerable()
                           where sp.NhaSanXuat == Nsx.Id && sp.Id == MaSua
                           select new doanweb.Models.tbl_sanpham
                           {
                               Id = sp.Id,
                               TenSanPham = sp.TenSanPham,
                               GiaTien = sp.GiaTien,
                               ChiTietSanPham = sp.ChiTietSanPham,
                               Tennhasanxuat = Nsx.TenNhaSanXuat,
                               LoaiSanPham = sp.LoaiSanPham,
                               AnhSanPham = sp.AnhSanPham
                           }).FirstOrDefault();
                return View(sps);
            }
            
        }
    }
}
