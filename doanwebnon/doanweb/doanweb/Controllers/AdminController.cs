using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doanweb.Models;
using doanweb.PublicClass;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace doanweb.Controllers
{
    public class AdminController : Controller
    {
        WebSiteBanNonEntities db = new WebSiteBanNonEntities();
        KiemTraChuoiClass ktc = new KiemTraChuoiClass();
        public ActionResult Index()
        {
            if(Session["taikhoan"]==null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if(Int32.Parse(Session["quyenhan"].ToString())!=2)
            {
                return RedirectToAction("Index", "Home");
            }

            
            //giỏ hàng trạng thái 1 là giỏ hàng , 2 là bấm thanh toán ,3 là thanh toán thành công ,4 là don hang lỗi
            var lstgiohang = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              where sp.Id == gh.Id_MaSanPham
                              select new doanweb.Models.tbl_sanpham
                              {
                                  NgayNhap = gh.NgayNhap,
                                  SoLuong = gh.SoLuongSanPham,
                                  GiaTien = sp.GiaTien,
                                  TrangThai=gh.TrangThai

                              }).ToList();
            double doanhthuthang = 0, doanhthunam = 0;
            int sodonchoduyet = 0, sodonhangloi = 0,tongso=0;
            var thangnow = DateTime.Now.Month.ToString();
            var namnow = DateTime.Now.Year.ToString();

            foreach(var item in lstgiohang)
            {
                if(item.NgayNhap.ToString().Substring(3,2)==thangnow && item.NgayNhap.ToString().Substring(6,4)==namnow && item.TrangThai==3)
                {
                    doanhthuthang += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if(item.NgayNhap.ToString().Substring(6,4)==namnow && item.TrangThai==3)
                {
                    doanhthunam += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if(item.TrangThai==2)
                {
                    sodonchoduyet++;
                }
                if(item.TrangThai==4)
                {
                    sodonhangloi++;
                }
                tongso++;
                
            }
            ViewBag.doanhthuthangA_id = (long)doanhthuthang;
            ViewBag.doanhthunamA_id = (long)doanhthunam;
            ViewBag.sodonchoduyetA_id = sodonchoduyet;
            if (tongso != 0)
            {
                ViewBag.sodonchoduyetA_Ve_id = Convert.ToInt32((sodonchoduyet / Convert.ToDouble(tongso)) * 100);
            }
            else
            {
                ViewBag.sodonchoduyetA_Ve_id = 0;
            }
            ViewBag.sodonhangloiA_id = sodonhangloi;

            ////////////////////////////////////////////////////////////////////////////////////////////////

            var lstgiohangdoanhthu = (from sp in db.tbl_sanpham.AsEnumerable()
                              from gh in db.tbl_giohang.AsEnumerable()
                              where sp.Id == gh.Id_MaSanPham && gh.TrangThai == 3
                              select new doanweb.Models.tbl_sanpham
                              {
                                  NgayNhap = gh.NgayNhap,
                                  SoLuong = gh.SoLuongSanPham,
                                  GiaTien = sp.GiaTien,
                                  TrangThai = gh.TrangThai

                              }).ToList();

            double thang1 = 0, thang2 = 0, thang3 = 0, thang4 = 0, thang5 = 0, thang6 = 0, thang7 = 0, thang8 = 0, thang9 = 0, thang10 = 0, thang11 = 0, thang12 = 0;

            foreach (var item in lstgiohangdoanhthu)
            {
                if (item.NgayNhap.ToString().Substring(3, 2) == "01" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang1 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "02" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang2 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "03" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang3 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "04" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang4 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "05" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang5 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "06" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang6 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "07" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang7 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "08" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang8 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "09" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang1 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "10" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang10 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "11" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang11 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
                if (item.NgayNhap.ToString().Substring(3, 2) == "12" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
                {
                    thang12 += Convert.ToDouble(item.GiaTien * item.SoLuong);
                }
            }
            ViewBag.doanhthuA_thang1 = (long)thang1;
            ViewBag.doanhthuA_thang2 = (long)thang2;
            ViewBag.doanhthuA_thang3 = (long)thang3;
            ViewBag.doanhthuA_thang4 = (long)thang4;
            ViewBag.doanhthuA_thang5 = (long)thang5;
            ViewBag.doanhthuA_thang6 = (long)thang6;
            ViewBag.doanhthuA_thang7 = (long)thang7;
            ViewBag.doanhthuA_thang8 = (long)thang8;
            ViewBag.doanhthuA_thang9 = (long)thang9;
            ViewBag.doanhthuA_thang10 = (long)thang10;
            ViewBag.doanhthuA_thang11 = (long)thang11;
            ViewBag.doanhthuA_thang12 = (long)thang12;

            ////////////////////////////////////////////////////////////////////////////////////////////

            int tongsoluongsanphambantrongthang = 0;

            string sp1 = "";
            long sp1id = 0;
            long slsp1 = 0;

            string sp2 = "";
            long sp2id = 0;
            long slsp2 = 0;

            string sp3 = "";
            long sp3id = 0;
            long slsp3 = 0;

            long slspcl = 0;

            var lstghbieudotron = (from sp in db.tbl_sanpham.AsEnumerable()
                         from gh in db.tbl_giohang.AsEnumerable()
                         where sp.Id == gh.Id_MaSanPham && gh.TrangThai == 3 && gh.NgayNhap.ToString().Substring(3, 2) == thangnow && gh.NgayNhap.ToString().Substring(6, 4) == namnow
                         select new doanweb.Models.tbl_giohang
                         {
                             Id = sp.Id,
                             Id_MaSanPham = gh.Id_MaSanPham,
                             TenSanPham = sp.TenSanPham,
                             NgayNhap = gh.NgayNhap,
                             SoLuongSanPham = gh.SoLuongSanPham

                         }).ToList();

            if (lstghbieudotron.Count != 0)
            {
                sp1id = 0;
                sp1 = "";
                slsp1 = 0;

                foreach (var item in lstghbieudotron)
                {
                    tongsoluongsanphambantrongthang += Convert.ToInt32(item.SoLuongSanPham);
                    if (slsp1 < ktslsp(lstghbieudotron, Convert.ToInt32(item.Id_MaSanPham)))
                    {
                        sp1id = Convert.ToInt32(item.Id_MaSanPham);
                        sp1 = item.TenSanPham;
                        slsp1 = ktslsp(lstghbieudotron, item.Id);
                    }

                }

                foreach (var item in lstghbieudotron)
                {
                    if (sp1id != item.Id_MaSanPham)
                    {
                        sp2id = Convert.ToInt32(item.Id_MaSanPham);
                        sp2 = item.TenSanPham;
                        slsp2 = Convert.ToInt32(item.SoLuongSanPham);
                        break;
                    }

                }

                foreach (var item in lstghbieudotron)
                {
                    if (sp1id != item.Id_MaSanPham)
                    {
                        if (slsp2 < ktslsp(lstghbieudotron, Convert.ToInt32(item.Id_MaSanPham)))
                        {
                            sp2id = Convert.ToInt32(item.Id_MaSanPham);
                            sp2 = item.TenSanPham;
                            slsp2 = ktslsp(lstghbieudotron, Convert.ToInt32(item.Id_MaSanPham));
                        }
                    }

                }


                foreach (var item in lstghbieudotron)
                {
                    if (sp1id != item.Id_MaSanPham && sp2id != item.Id_MaSanPham)
                    {
                        sp3id = Convert.ToInt32(item.Id_MaSanPham);
                        sp3 = item.TenSanPham;
                        slsp3 = Convert.ToInt32(item.SoLuongSanPham);
                        break;
                    }

                }

                foreach (var item in lstghbieudotron)
                {
                    if (sp1id != item.Id_MaSanPham && sp2id != item.Id_MaSanPham)
                    {
                        if (slsp3 < ktslsp(lstghbieudotron, Convert.ToInt32(item.Id_MaSanPham)))
                        {
                            sp3id = Convert.ToInt32(item.Id_MaSanPham);
                            sp3 = item.TenSanPham;
                            slsp3 = ktslsp(lstghbieudotron, Convert.ToInt32(item.Id_MaSanPham));
                        }
                    }

                }

                slspcl = tongsoluongsanphambantrongthang - (slsp1 + slsp2 + slsp3);

                ViewBag.BieuDoTronA_sp1 = sp1;
                ViewBag.BieuDoTronA_slsp1 = Convert.ToInt32((slsp1 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);


                ViewBag.BieuDoTronA_sp2 = sp2;
                ViewBag.BieuDoTronA_slsp2 = Convert.ToInt32((slsp2 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);

                ViewBag.BieuDoTronA_sp3 = sp3;
                ViewBag.BieuDoTronA_slsp3 = Convert.ToInt32((slsp3 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);

                ViewBag.BieuDoTronA_slspcl = Convert.ToInt32((slspcl / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);
            }
            else
            {
                ViewBag.BieuDoTronA_sp1 = sp1;
                ViewBag.BieuDoTronA_slsp1 = 0;


                ViewBag.BieuDoTronA_sp2 = sp2;
                ViewBag.BieuDoTronA_slsp2 = 0;

                ViewBag.BieuDoTronA_sp3 = sp3;
                ViewBag.BieuDoTronA_slsp3 = 0;

                ViewBag.BieuDoTronA_slspcl = 0;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////

            long tongsoluongdonhang = 0, giohang = 0, donhang = 0, donhangthanhcong = 0, donhangthatbai = 0;

            var lstgh = (from gh in db.tbl_giohang.AsEnumerable()
                         where gh.NgayNhap.ToString().Substring(3, 2) == thangnow && gh.NgayNhap.ToString().Substring(6, 4) == namnow
                         select new doanweb.Models.tbl_giohang
                         {
                             TrangThai = gh.TrangThai

                         }).ToList();
            if (lstgh.Count != 0)
            {
                tongsoluongdonhang = lstgh.Count;
                foreach (var item in lstgh)
                {
                    if (item.TrangThai == 1)
                    {
                        giohang += 1;
                    }
                    else if (item.TrangThai == 2)
                    {
                        donhang += 1;
                    }
                    else if (item.TrangThai == 3)
                    {
                        donhangthanhcong += 1;
                    }
                    else if (item.TrangThai == 4)
                    {
                        donhangthatbai += 1;
                    }
                }
                ViewBag.BangSoLieu_Admin_giohang = Convert.ToInt64((giohang / Convert.ToDouble(tongsoluongdonhang)) * 100);
                ViewBag.BangSoLieu_Admin_donhang = Convert.ToInt64((donhang / Convert.ToDouble(tongsoluongdonhang)) * 100);
                ViewBag.BangSoLieu_Admin_donhangthanhcong = Convert.ToInt64((donhangthanhcong / Convert.ToDouble(tongsoluongdonhang)) * 100);
                ViewBag.BangSoLieu_Admin_donhangthatbai = Convert.ToInt64((donhangthatbai / Convert.ToDouble(tongsoluongdonhang)) * 100);
            }
            else
            {
                ViewBag.BangSoLieu_Admin_giohang = 0;
                ViewBag.BangSoLieu_Admin_donhang = 0;
                ViewBag.BangSoLieu_Admin_donhangthanhcong = 0;
                ViewBag.BangSoLieu_Admin_donhangthatbai = 0;
            }



            return View();
        }

        public int ktslsp(List<tbl_giohang> lstgh, int masp)
        {
            int sl = 0;
            foreach (var item in lstgh)
            {
                if (item.Id_MaSanPham == masp)
                {
                    sl += Convert.ToInt32(item.SoLuongSanPham);
                }
            }
            return sl;
        }

        //public ActionResult BieuDoDoanhThu_Admin_PartialView()
        //{
        //    if (Session["taikhoan"] == null)
        //    {
        //        return RedirectToAction("DangNhap", "DangNhap");
        //    }

        //    if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    BieuDoDoanhThu bd = new BieuDoDoanhThu();
        //    var lstgiohang = (from sp in db.tbl_sanpham.AsEnumerable()
        //                      from gh in db.tbl_giohang.AsEnumerable()
        //                      where sp.Id == gh.Id_MaSanPham && gh.TrangThai==3
        //                      select new doanweb.Models.tbl_sanpham
        //                      {
        //                          NgayNhap = gh.NgayNhap,
        //                          SoLuong = gh.SoLuongSanPham,
        //                          GiaTien = sp.GiaTien,
        //                          TrangThai = gh.TrangThai

        //                      }).ToList();

        //    double  thang1 = 0, thang2 = 0, thang3 = 0, thang4 = 0, thang5 = 0, thang6 = 0, thang7 = 0, thang8 = 0, thang9 = 0, thang10 = 0, thang11 = 0, thang12 = 0;
        //    var thangnow = DateTime.Now.Month.ToString();
        //    var namnow = DateTime.Now.Year.ToString();
        //    foreach(var item in lstgiohang)
        //    {
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "01" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang1 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "02" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang2 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "03" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang3 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "04" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang4 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "05" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang5 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "06" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang6 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "07" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang7 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "08" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang8 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "09" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang1 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "10" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang10 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "11" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang11 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //        if (item.NgayNhap.ToString().Substring(3, 2) == "12" && item.NgayNhap.ToString().Substring(6, 4) == namnow && item.TrangThai == 3)
        //        {
        //            thang12 += Convert.ToDouble(item.GiaTien * item.SoLuong);
        //        }
        //    }
        //    bd.doanhthuA_thang1 =(long) thang1;
        //    bd.doanhthuA_thang2 = (long)thang2;
        //    bd.doanhthuA_thang3 = (long)thang3;
        //    bd.doanhthuA_thang4 = (long)thang4;
        //    bd.doanhthuA_thang5 = (long)thang5;
        //    bd.doanhthuA_thang6 = (long)thang6;
        //    bd.doanhthuA_thang7 = (long)thang7;
        //    bd.doanhthuA_thang8 = (long)thang8;
        //    bd.doanhthuA_thang9 = (long)thang9;
        //    bd.doanhthuA_thang10 = (long)thang10;
        //    bd.doanhthuA_thang11 = (long)thang11;
        //    bd.doanhthuA_thang12 = (long)thang12;

        //    return PartialView(bd);
        //}
        //public int ktslsp(List<tbl_giohang> lstgh,int masp)
        //{
        //    int sl=0;
        //    foreach(var item in lstgh)
        //    {
        //        if (item.Id_MaSanPham == masp)
        //        {
        //            sl +=Convert.ToInt32( item.SoLuongSanPham);
        //        }
        //    }
        //    return sl;
        //}
        //public ActionResult BieuDoTron_Admin_PartialView()
        //{
        //    if (Session["taikhoan"] == null)
        //    {
        //        return RedirectToAction("DangNhap", "DangNhap");
        //    }

        //    if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    BieuDoTron bd = new BieuDoTron();

        //    var thangnow = DateTime.Now.Month.ToString();
        //    var namnow = DateTime.Now.Year.ToString();


        //    int  tongsoluongsanphambantrongthang = 0;

        //    string sp1 = "";
        //    long sp1id=0;
        //    long slsp1 = 0;

        //    string sp2 = "";
        //    long sp2id = 0;
        //    long slsp2 = 0;

        //    string sp3 = "";
        //    long sp3id = 0;
        //    long slsp3 = 0;

        //    long slspcl = 0;

        //    var lstgh = (from sp in db.tbl_sanpham.AsEnumerable()
        //                      from gh in db.tbl_giohang.AsEnumerable()
        //                 where sp.Id == gh.Id_MaSanPham && gh.TrangThai == 3 && gh.NgayNhap.ToString().Substring(3, 2) == thangnow && gh.NgayNhap.ToString().Substring(6, 4) == namnow
        //                      select new doanweb.Models.tbl_giohang
        //                      {
        //                          Id=sp.Id,
        //                          Id_MaSanPham=gh.Id_MaSanPham,
        //                          TenSanPham=sp.TenSanPham,
        //                          NgayNhap = gh.NgayNhap,
        //                          SoLuongSanPham = gh.SoLuongSanPham

        //                      }).ToList();
            
        //    if(lstgh.Count!=0)
        //    {
        //        sp1id = 0;
        //        sp1 = "";
        //        slsp1 = 0;

        //        foreach (var item in lstgh)
        //        {
        //            tongsoluongsanphambantrongthang += Convert.ToInt32(item.SoLuongSanPham);
        //            if (slsp1 < ktslsp(lstgh, Convert.ToInt32(item.Id_MaSanPham)))
        //            {
        //                sp1id = Convert.ToInt32(item.Id_MaSanPham);
        //                sp1 = item.TenSanPham;
        //                slsp1 = ktslsp(lstgh, item.Id);
        //            }

        //        }

        //        foreach (var item in lstgh)
        //        {
        //            if (sp1id != item.Id_MaSanPham)
        //            {
        //                sp2id = Convert.ToInt32(item.Id_MaSanPham);
        //                sp2 = item.TenSanPham;
        //                slsp2 = Convert.ToInt32(item.SoLuongSanPham);
        //                break;
        //            }

        //        }

        //        foreach (var item in lstgh)
        //        {
        //            if (sp1id != item.Id_MaSanPham)
        //            {
        //                if (slsp2 < ktslsp(lstgh, Convert.ToInt32(item.Id_MaSanPham)))
        //                {
        //                    sp2id = Convert.ToInt32(item.Id_MaSanPham);
        //                    sp2 = item.TenSanPham;
        //                    slsp2 = ktslsp(lstgh, Convert.ToInt32(item.Id_MaSanPham));
        //                }
        //            }

        //        }


        //        foreach (var item in lstgh)
        //        {
        //            if (sp1id != item.Id_MaSanPham && sp2id != item.Id_MaSanPham)
        //            {
        //                sp3id = Convert.ToInt32(item.Id_MaSanPham);
        //                sp3 = item.TenSanPham;
        //                slsp3 = Convert.ToInt32(item.SoLuongSanPham);
        //                break;
        //            }

        //        }

        //        foreach (var item in lstgh)
        //        {
        //            if (sp1id != item.Id_MaSanPham && sp2id != item.Id_MaSanPham)
        //            {
        //                if (slsp3 < ktslsp(lstgh, Convert.ToInt32(item.Id_MaSanPham)))
        //                {
        //                    sp3id = Convert.ToInt32(item.Id_MaSanPham);
        //                    sp3 = item.TenSanPham;
        //                    slsp3 = ktslsp(lstgh, Convert.ToInt32(item.Id_MaSanPham));
        //                }
        //            }

        //        }

        //        slspcl = tongsoluongsanphambantrongthang - (slsp1 + slsp2 + slsp3);

        //        bd.BieuDoTronA_sp1 = sp1;
        //        bd.BieuDoTronA_slsp1 = Convert.ToInt32((slsp1 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);


        //        bd.BieuDoTronA_sp2 = sp2;
        //        bd.BieuDoTronA_slsp2 = Convert.ToInt32((slsp2 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);

        //        bd.BieuDoTronA_sp3 = sp3;
        //        bd.BieuDoTronA_slsp3 = Convert.ToInt32((slsp3 / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);

        //        bd.BieuDoTronA_slspcl = Convert.ToInt32((slspcl / Convert.ToDouble(tongsoluongsanphambantrongthang)) * 100);
        //    }
        //    else
        //    {
        //        bd.BieuDoTronA_sp1 = sp1;
        //        bd.BieuDoTronA_slsp1 = 0;


        //        bd.BieuDoTronA_sp2 = sp2;
        //        bd.BieuDoTronA_slsp2 = 0;

        //        bd.BieuDoTronA_sp3 = sp3;
        //        bd.BieuDoTronA_slsp3 = 0;

        //        bd.BieuDoTronA_slspcl = 0;
        //    }

        //    return PartialView(bd);
        //}

        //public ActionResult BanPhanTichSoLieu_Admin_PartialView()
        //{
        //    if (Session["taikhoan"] == null)
        //    {
        //        return RedirectToAction("DangNhap", "DangNhap");
        //    }

        //    if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    BangPhanTichSoLieu bd = new BangPhanTichSoLieu();

        //    var thangnow = DateTime.Now.Month.ToString();
        //    var namnow = DateTime.Now.Year.ToString();
        //    long tongsoluongdonhang = 0,giohang=0,donhang=0,donhangthanhcong=0,donhangthatbai=0;

        //    var lstgh = (from gh in db.tbl_giohang.AsEnumerable()
        //                 where gh.NgayNhap.ToString().Substring(3,2)==thangnow && gh.NgayNhap.ToString().Substring(6,4)==namnow
        //                 select new doanweb.Models.tbl_giohang
        //                 {
        //                     TrangThai=gh.TrangThai

        //                 }).ToList();
        //    if(lstgh.Count!=0)
        //    {
        //        tongsoluongdonhang = lstgh.Count;
        //        foreach (var item in lstgh)
        //        {
        //            if (item.TrangThai == 1)
        //            {
        //                giohang += 1;
        //            }
        //            else if (item.TrangThai == 2)
        //            {
        //                donhang += 1;
        //            }
        //            else if (item.TrangThai == 3)
        //            {
        //                donhangthanhcong += 1;
        //            }
        //            else if (item.TrangThai == 4)
        //            {
        //                donhangthatbai += 1;
        //            }
        //        }
        //        bd.BangSoLieu_Admin_giohang = Convert.ToInt64((giohang / Convert.ToDouble(tongsoluongdonhang)) * 100);
        //        bd.BangSoLieu_Admin_donhang = Convert.ToInt64((donhang / Convert.ToDouble(tongsoluongdonhang)) * 100);
        //        bd.BangSoLieu_Admin_donhangthanhcong = Convert.ToInt64((donhangthanhcong / Convert.ToDouble(tongsoluongdonhang)) * 100);
        //        bd.BangSoLieu_Admin_donhangthatbai = Convert.ToInt64((donhangthatbai / Convert.ToDouble(tongsoluongdonhang)) * 100);
        //    }
        //    else
        //    {
        //        bd.BangSoLieu_Admin_giohang = 0;
        //        bd.BangSoLieu_Admin_donhang = 0;
        //        bd.BangSoLieu_Admin_donhangthanhcong = 0;
        //        bd.BangSoLieu_Admin_donhangthatbai = 0;
        //    }

        //    return PartialView(bd);
        //}

        public ActionResult KhachHangMoi_Admin_PartialView()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var lstkh = db.tbl_member.OrderByDescending(n => n.Id).Take(6).ToList();
            return PartialView(lstkh);
        }

        public ActionResult QuanLyNguoiDung()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            int taiKhoan = Int32.Parse(Session["taikhoan"].ToString());
            var lstnguoidung = db.tbl_member.Where(n=>n.Id!=taiKhoan).ToList();
            return View(lstnguoidung);
        }
        [HttpGet]
        public ActionResult QuanLyNguoiDung_Them()
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            

            ViewBag.QLND_T_TieuDe = null;
            ViewBag.QLND_T_TenDangNhap = null;
            ViewBag.QLND_T_MatKhau = null;
            ViewBag.QLND_T_Email = null;
            ViewBag.QLND_T_TenKhachHang = null;
            ViewBag.QLND_T_NgaySinh = null;
            ViewBag.QLND_T_DiaChi = null;
            ViewBag.QLND_T_SDT = null;
            ViewBag.QLND_T_QuyenHan = null;
            return View();
        }
        [HttpPost]
        public ActionResult QuanLyNguoiDung_Them(tbl_member kh,FormCollection f)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            

            if(kh.TenDangNhap==null)
            {
                ViewBag.QLND_T_TenDangNhap = "( Tên Đăng Nhập Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_T_TenDangNhap = null;
            }
            if(kh.MatKhau==null)
            {
                ViewBag.QLND_T_MatKhau = "( Mật Khẩu Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_T_MatKhau = null;
            }
            if(kh.Email==null)
            {
                ViewBag.QLND_T_Email = "( Email Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_T_Email = null;
            }
            if(kh.TenKhachHang==null)
            {
                ViewBag.QLND_T_TenKhachHang = "( Tên Khách Hàng Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_T_TenKhachHang = null;
            }
            if(f.Get("QuyenHan")=="")
            {
                ViewBag.QLND_T_QuyenHan = "( Lỗi Quyền Hạn ,Load Lại Trang )";
            }
            else
            {
                if(Convert.ToInt32(f.Get("QuyenHan"))!=1 && Convert.ToInt32(f.Get("QuyenHan"))!=2)
                {
                    ViewBag.QLND_T_QuyenHan = "( Lỗi Quyền Hạn Do Bạn Cố Gắng Thay Đổi Code Html Của Website ! Hãy Load Lại Trang )";
                }
                else
                {
                    ViewBag.QLND_T_QuyenHan = null;
                }
                
            }

            if (ViewBag.QLND_T_TenKhachHang != null || ViewBag.QLND_T_TenDangNhap != null || ViewBag.QLND_T_MatKhau != null || ViewBag.QLND_T_Email != null || ViewBag.QLND_T_QuyenHan != null)
            {
                return View();
            }

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(kh.TenKhachHang.ToString());
            if (ketqua == true)
            {
                ViewBag.QLND_T_TenKhachHang = null;
            }
            else
            {
                ViewBag.QLND_T_TenKhachHang = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if (kh.NgaySinh == null)
            {
                ViewBag.QLND_T_NgaySinh = null;
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
                    ViewBag.QLND_T_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                }
                else if (int.Parse(namnhap) == int.Parse(namnow))
                {
                    if (int.Parse(thangnhap) > int.Parse(thangnow))
                    {
                        ViewBag.QLND_T_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                    }
                    else if (int.Parse(thangnhap) == int.Parse(thangnow))
                    {
                        if (int.Parse(ngaynhap) >= int.Parse(ngaynow))
                        {
                            ViewBag.QLND_T_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                        }
                        else
                        {
                            ViewBag.QLND_T_NgaySinh = null;
                        }
                    }
                    else
                    {
                        ViewBag.QLND_T_NgaySinh = null;
                    }
                }
                else
                {
                    ViewBag.QLND_T_NgaySinh = null;
                }
            }


            ketqua = ktc.hamcatchuoi(kh.TenDangNhap.ToString());
            if (ketqua == false)
            {
                ViewBag.QLND_T_TenDangNhap = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
            }
            else
            {

                ViewBag.QLND_T_TenDangNhap = null;
            }

            if (kh.MatKhau == null)
            {
                ViewBag.QLND_T_MatKhau = null;
            }
            else
            {
                ketqua = ktc.hamkiemtradomanhmatkhau(kh.MatKhau.ToString());
                if (ketqua == false)
                {
                    ViewBag.QLND_T_MatKhau = "( Mật Khẩu Không Đủ Mạnh,Mật Khẩu Phải Trên 8 Ký Tự,Có Ký 1 Tự Hoa,Thường,Số,Và Ký Tự Đặc Biệt Vd: NguyenVanA@&5674)";
                }
                else
                {
                    ViewBag.QLND_T_MatKhau = null;
                }
            }

            ketqua = ktc.Kiemtraemail(kh.Email);
            if (ketqua == false)
            {
                ViewBag.QLND_T_Email  = "( Vui Lòng Nhập Đúng Định Dạng Email Vd: Google@gmail.com )";
            }
            else
            {
                ViewBag.QLND_T_Email = null;
            }

            if (kh.SDT == null)
            {
                ViewBag.QLND_T_SDT = null;
            }
            else
            {
                string sdt = kh.SDT.ToString().Replace("  ", string.Empty);
                ketqua = Regex.IsMatch(sdt, @"\D");
                if (ketqua == true)
                {
                    ViewBag.QLND_T_SDT = "(Kiểu Số Điện Thoại Không Đúng)";
                }
                else
                {
                    if (sdt.Length > 11 || sdt.Length < 10)
                    {
                        ViewBag.QLND_T_SDT = "(Số Điện Thoại Không Quá 11 Số Và Không Ít Hơn 10 Số)";
                    }
                    else
                    {
                        ViewBag.QLND_T_SDT = null;
                    }
                }
            }

            if (kh.DiaChi == null)
            {
                ViewBag.QLND_T_DiaChi = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(kh.DiaChi.ToString());
                if (ketqua == false)
                {
                    ViewBag.QLND_T_DiaChi = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
                else
                {
                    ViewBag.QLND_T_DiaChi = null;
                }
            }


            if (ViewBag.QLND_T_TenKhachHang != null || ViewBag.QLND_T_NgaySinh != null || ViewBag.QLND_T_TenDangNhap != null || ViewBag.QLND_T_MatKhau != null || ViewBag.QLND_T_Email != null || ViewBag.QLND_T_SDT != null || ViewBag.QLND_T_DiaChi != null || ViewBag.QLND_T_QuyenHan != null)
            {
                return View();
            }


            //đầu tiên là kiểm tra tên đăng nhập này có tồn tại với id khác không,có nghĩa là tài khoản khác có tên đăng nhập này,nếu không thì sẽ 
            //kiểm tra tiếp ,là tên đăng nhập này có tồn tại với id tài khoản này không,nếu không tồn tại thì có nghĩa là tài khoản này sửa  tên đăng nhập
            //ngược lại có tồn tại có nghĩa tài khoản này k sửa tên đăng nhập
            var kttk = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == kh.TenDangNhap);
            if (kttk == null)
            {
                tbl_member them = new tbl_member();
                string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(kh.MatKhau, "MD5");

                if (kh.SDT != null)
                {
                    string sdt = kh.SDT.ToString().Replace("  ", "");
                    them.SDT = sdt;
                }

                if (kh.NgaySinh != null)
                {
                    them.NgaySinh = kh.NgaySinh;
                }

                if (kh.DiaChi != null)
                {
                    them.DiaChi = kh.DiaChi;
                }
                them.MatKhau = mahoamatkhau;
                them.Email = kh.Email;
                them.TenDangNhap = kh.TenDangNhap;
                them.TenKhachHang = kh.TenKhachHang;
                them.QuyenHan =Convert.ToInt32( f.Get("QuyenHan"));

                db.tbl_member.Add(them);

                ///////////////////////////////////////////////////////////////////////////////////
                tbl_Hmember moi = new tbl_Hmember();

                moi.TenDangNhap = kh.TenDangNhap;
                moi.TenDangNhap_Cu = kh.TenDangNhap;
                moi.MatKhau = them.MatKhau;
                moi.MatKhau_Cu = them.MatKhau;

                if (Convert.ToInt32(f.Get("QuyenHan"))==1)
                {
                    moi.QuyenHan = "Khách Hàng";
                    moi.QuyenHan_Cu = "Khách Hàng";
                }
                else
                {
                    moi.QuyenHan = "Admin";
                    moi.QuyenHan_Cu = "Admin";
                }
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
                moi.TrangThai = "Thêm Tài Khoản Người Dùng - Admin";
                db.tbl_Hmember.Add(moi);

                //////////////////////////////////////////////////////////////////////////////////


                db.SaveChanges();
                ViewBag.QLND_T_TieuDe = "(LƯU THÀNH CÔNG)";
                return View();
            }
            else
            {
                ViewBag.QLND_T_TieuDe = "(TÊN ĐĂNG NHẬP CỦA TÀI KHOẢN ĐÃ TỒN TẠI)";
                return View();
            }

        }


        public ActionResult QuanLyNguoiDung_Xoa(int MaXoa=0 )
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            tbl_member qlndx = db.tbl_member.FirstOrDefault(n => n.Id == MaXoa);
            if(qlndx!=null)
            {
                db.tbl_member.Remove(qlndx);
                var lstgh = db.tbl_giohang.Where(n => n.Id_user == MaXoa && (n.TrangThai == 1 || n.TrangThai==2));
                foreach(var item in lstgh)
                {
                    db.tbl_giohang.Remove(item);
                }

                ///////////////////////////////////////////////////////////////////////////////////
                tbl_Hmember moi = new tbl_Hmember();

                moi.TenDangNhap = qlndx.TenDangNhap;
                moi.TenDangNhap_Cu = qlndx.TenDangNhap;
                moi.MatKhau = qlndx.MatKhau;
                moi.MatKhau_Cu = qlndx.MatKhau;

                if (qlndx.QuyenHan == 1)
                {
                    moi.QuyenHan = "Khách Hàng";
                    moi.QuyenHan_Cu = "Khách Hàng";
                }
                else
                {
                    moi.QuyenHan = "Admin";
                    moi.QuyenHan_Cu = "Admin";
                }
                moi.TenKhachHang = qlndx.TenKhachHang;
                moi.TenKhachHang_Cu = qlndx.TenKhachHang;
                moi.NgaySinh = qlndx.NgaySinh;
                moi.NgaySinh_Cu = qlndx.NgaySinh;
                moi.SDT = qlndx.SDT;
                moi.SDT_Cu = qlndx.SDT;
                moi.DiaChi = qlndx.DiaChi;
                moi.DiaChi_Cu = qlndx.DiaChi;
                moi.Email = qlndx.Email;
                moi.Email_Cu = qlndx.Email;
                moi.NguoiThayDoi = qlndx.TenDangNhap;
                moi.NgayThayDoi = DateTime.Now;
                moi.TrangThai = "Xóa Tài Khoản Người Dùng - Admin";
                db.tbl_Hmember.Add(moi);

                //////////////////////////////////////////////////////////////////////////////////


                db.SaveChanges();
            }
            
            return RedirectToAction("QuanLyNguoiDung","Admin");
        }

        [HttpGet]
        public ActionResult QuanLyNguoiDung_Sua(int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.QLND_S_TieuDe = null;
            ViewBag.QLND_S_TenDangNhap = null;
            ViewBag.QLND_S_MatKhau = null;
            ViewBag.QLND_S_Email = null;
            ViewBag.QLND_S_TenKhachHang = null;
            ViewBag.QLND_S_NgaySinh = null;
            ViewBag.QLND_S_DiaChi = null;
            ViewBag.QLND_S_SDT = null;
            ViewBag.QLND_S_QuyenHan = null;

            var lstqlnd = db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
            lstqlnd.MatKhau = null;
            return View(lstqlnd);
        }
        [HttpPost]
        public ActionResult QuanLyNguoiDung_Sua(tbl_member kh, FormCollection f,int MaSua=0)
        {
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }

            if (Int32.Parse(Session["quyenhan"].ToString()) != 2)
            {
                return RedirectToAction("Index", "Home");
            }

            int taikhoan = Int32.Parse(Session["taikhoan"].ToString());

            if (kh.TenDangNhap == null)
            {
                ViewBag.QLND_S_TenDangNhap = "( Tên Đăng Nhập Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_S_TenDangNhap = null;
            }


            if (kh.Email == null)
            {
                ViewBag.QLND_S_Email = "( Email Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_S_Email = null;
            }

            if (kh.TenKhachHang == null)
            {
                ViewBag.QLND_S_TenKhachHang = "( Tên Khách Hàng Không Được Bỏ Trống )";
            }
            else
            {
                ViewBag.QLND_S_TenKhachHang = null;
            }

            if (f.Get("QuyenHan") == "")
            {
                ViewBag.QLND_S_QuyenHan = "( Lỗi Quyền Hạn ,Load Lại Trang )";
            }
            else
            {
                if (Convert.ToInt32(f.Get("QuyenHan"))!=1 && Convert.ToInt32(f.Get("QuyenHan"))!=2)
                {
                    ViewBag.QLND_S_QuyenHan = "( Lỗi Quyền Hạn Do Bạn Cố Gắng Thay Đổi Code Html Của Website ! Hãy Load Lại Trang )";
                }
                else{
                    ViewBag.QLND_S_QuyenHan = null;
                }
            }

            if (ViewBag.QLND_S_TenKhachHang != null || ViewBag.QLND_S_TenDangNhap != null || ViewBag.QLND_S_MatKhau != null || ViewBag.QLND_S_Email != null || ViewBag.QLND_S_QuyenHan != null)
            {
                var lstqlnd = db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
                lstqlnd.MatKhau = null;
                return View(lstqlnd);
            }

            bool ketqua = true;
            ketqua = ktc.hamcatchuoi(kh.TenKhachHang.ToString());
            if (ketqua == true)
            {
                ViewBag.QLND_S_TenKhachHang = null;
            }
            else
            {
                ViewBag.QLND_S_TenKhachHang = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
            }

            if (kh.NgaySinh == null)
            {
                ViewBag.QLND_S_NgaySinh = null;
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
                    ViewBag.QLND_S_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                }
                else if (int.Parse(namnhap) == int.Parse(namnow))
                {
                    if (int.Parse(thangnhap) > int.Parse(thangnow))
                    {
                        ViewBag.QLND_S_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                    }
                    else if (int.Parse(thangnhap) == int.Parse(thangnow))
                    {
                        if (int.Parse(ngaynhap) >= int.Parse(ngaynow))
                        {
                            ViewBag.QLND_S_NgaySinh = "(Ngày Sinh Không Được Lớn Hơn Ngày Hiện Tại)";
                        }
                        else
                        {
                            ViewBag.QLND_S_NgaySinh = null;
                        }
                    }
                    else
                    {
                        ViewBag.QLND_S_NgaySinh = null;
                    }
                }
                else
                {
                    ViewBag.QLND_S_NgaySinh = null;
                }
            }


            ketqua = ktc.hamcatchuoi(kh.TenDangNhap.ToString());
            if (ketqua == false)
            {
                ViewBag.QLND_S_TenDangNhap = "(Vui Lòng Không Nhập Các Ký Tự Đặc Biệt)";
            }
            else
            {

                ViewBag.QLND_S_TenDangNhap = null;
            }

            if (kh.MatKhau == null)
            {
                ViewBag.QLND_S_MatKhau = null;
            }
            else
            {
                ketqua = ktc.hamkiemtradomanhmatkhau(kh.MatKhau.ToString());
                if (ketqua == false)
                {
                    ViewBag.QLND_S_MatKhau = "( Mật Khẩu Không Đủ Mạnh,Mật Khẩu Phải Trên 8 Ký Tự,Có Ký 1 Tự Hoa,Thường,Số,Và Ký Tự Đặc Biệt Vd: NguyenVanA@&5674)";
                }
                else
                {
                    ViewBag.QLND_S_MatKhau = null;
                }
            }

            ketqua = ktc.Kiemtraemail(kh.Email.ToString());
            if (ketqua == false)
            {
                ViewBag.QLND_S_Email = "( Vui Lòng Nhập Đúng Định Dạng Email Vd: Google@gmail.com )";
            }
            else
            {
                ViewBag.QLND_S_Email = null;
            }

            if (kh.SDT == null)
            {
                ViewBag.QLND_S_SDT = null;
            }
            else
            {
                string sdt = kh.SDT.ToString().Replace("  ", string.Empty);
                ketqua = Regex.IsMatch(sdt, @"\D");
                if (ketqua == true)
                {
                    ViewBag.QLND_S_SDT = "(Kiểu Số Điện Thoại Không Đúng)";
                }
                else
                {
                    if (sdt.Length > 11 || sdt.Length < 10)
                    {
                        ViewBag.QLND_S_SDT = "(Số Điện Thoại Không Quá 11 Số Và Không Ít Hơn 10 Số)";
                    }
                    else
                    {
                        ViewBag.QLND_S_SDT = null;
                    }
                }
            }

            if (kh.DiaChi == null)
            {
                ViewBag.QLND_S_DiaChi = null;
            }
            else
            {
                ketqua = ktc.hamcatchuoi(kh.DiaChi.ToString());
                if (ketqua == false)
                {
                    ViewBag.QLND_S_DiaChi = "( Vui Lòng Không Nhập Các Ký Tự Đặc Biệt )";
                }
                else
                {
                    ViewBag.QLND_S_DiaChi = null;
                }
            }


            if (ViewBag.QLND_S_TenKhachHang != null || ViewBag.QLND_S_NgaySinh != null || ViewBag.QLND_S_TenDangNhap != null || ViewBag.QLND_S_MatKhau != null || ViewBag.QLND_S_Email != null || ViewBag.QLND_S_SDT != null || ViewBag.QLND_S_DiaChi != null || ViewBag.QLND_S_QuyenHan != null)
            {
                var lstqlnd = db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
                lstqlnd.MatKhau = null;
                return View(lstqlnd);
            }


            //đầu tiên là kiểm tra tên đăng nhập này có tồn tại với id khác không,có nghĩa là tài khoản khác có tên đăng nhập này,nếu không thì sẽ 
            //kiểm tra tiếp ,là tên đăng nhập này có tồn tại với id tài khoản này không,nếu không tồn tại thì có nghĩa là tài khoản này sửa  tên đăng nhập
            //ngược lại có tồn tại có nghĩa tài khoản này k sửa tên đăng nhập
            var kttk = db.tbl_member.FirstOrDefault(n => n.TenDangNhap == kh.TenDangNhap && n.Id!=MaSua );
            if (kttk == null)
            {
                ///////////////////////////////////////////////////////////////////////////////////
                tbl_Hmember moi = new tbl_Hmember();
                var ndlichsu= db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
                var adlichsu = db.tbl_member.FirstOrDefault(n => n.Id == taikhoan);

                tbl_member sua = db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
                
                if (kh.MatKhau != null)
                {
                    string mahoamatkhau = FormsAuthentication.HashPasswordForStoringInConfigFile(kh.MatKhau, "MD5");
                    moi.MatKhau = mahoamatkhau;
                    moi.MatKhau_Cu = ndlichsu.MatKhau;

                    sua.MatKhau = mahoamatkhau;
                }
                else
                {
                    moi.MatKhau = ndlichsu.MatKhau;
                    moi.MatKhau_Cu = ndlichsu.MatKhau;
                }
                if (kh.SDT != null)
                {
                    string sdt = kh.SDT.ToString().Replace("  ", "");
                    
                    moi.SDT = sdt;
                    moi.SDT_Cu = ndlichsu.SDT;

                    sua.SDT = sdt;
                }
                else
                {
                    moi.SDT = ndlichsu.SDT;
                    moi.SDT_Cu = ndlichsu.SDT;
                }

                if (kh.NgaySinh != null)
                {
                    moi.NgaySinh = kh.NgaySinh;

                    moi.NgaySinh_Cu = ndlichsu.NgaySinh;

                    sua.NgaySinh = kh.NgaySinh;
                }
                else
                {
                    moi.NgaySinh = ndlichsu.NgaySinh;
                    moi.NgaySinh_Cu = ndlichsu.NgaySinh;
                }

                if (kh.DiaChi != null)
                {
                    moi.DiaChi = kh.DiaChi;
                    moi.DiaChi_Cu = ndlichsu.DiaChi;

                    sua.DiaChi = kh.DiaChi;
                }
                else
                {
                    moi.DiaChi = ndlichsu.DiaChi;
                    moi.DiaChi_Cu = ndlichsu.DiaChi;
                }
                
                if(kh.TenDangNhap!=null)
                {
                    moi.TenDangNhap = kh.TenDangNhap;
                    moi.TenDangNhap_Cu = ndlichsu.TenDangNhap;
                }
                else
                {
                    moi.TenDangNhap = ndlichsu.TenDangNhap;
                    moi.TenDangNhap_Cu = ndlichsu.TenDangNhap;
                }


                if (Convert.ToInt32(f.Get("QuyenHan")) !=ndlichsu.QuyenHan)
                {
                    moi.QuyenHan = (Convert.ToInt32(f.Get("QuyenHan")) == 1) ? "Khách Hàng" : "Admin";
                    moi.QuyenHan_Cu = (ndlichsu.QuyenHan == 1) ? "Khách Hàng" : "Admin";
                }
                else
                {
                    moi.QuyenHan = (ndlichsu.QuyenHan == 1) ? "Khách Hàng" : "Admin";
                    moi.QuyenHan_Cu = (ndlichsu.QuyenHan == 1) ? "Khách Hàng" : "Admin";
                }
                
                if(kh.TenKhachHang!=null)
                {
                    moi.TenKhachHang = kh.TenKhachHang;
                    moi.TenKhachHang_Cu = ndlichsu.TenKhachHang;
                }
                else
                {
                    moi.TenKhachHang = ndlichsu.TenKhachHang;
                    moi.TenKhachHang_Cu = ndlichsu.TenKhachHang;
                }

                if(kh.Email!=null)
                {
                    moi.Email = kh.Email;
                    moi.Email_Cu = ndlichsu.Email;
                }
                else
                {
                    moi.Email = ndlichsu.Email;
                    moi.Email_Cu = ndlichsu.Email;
                }

                moi.NguoiThayDoi = adlichsu.TenDangNhap;
                moi.NgayThayDoi = DateTime.Now;
                moi.TrangThai = "Sửa Tài Khoản Người Dùng - Admin";
                db.tbl_Hmember.Add(moi);

                //////////////////////////////////////////////////////////////////////////////////

                sua.Email = kh.Email;
                sua.TenDangNhap = kh.TenDangNhap;
                sua.TenKhachHang = kh.TenKhachHang;
                sua.QuyenHan = Convert.ToInt32(f.Get("QuyenHan"));

                db.SaveChanges();
                ViewBag.QLND_S_TieuDe = "(CẬP NHẬT THÀNH CÔNG)";

                sua.MatKhau = null;
                return View(sua);
            }
            else
            {
                ViewBag.QLND_S_TieuDe = "(TÊN ĐĂNG NHẬP CỦA TÀI KHOẢN ĐÃ TỒN TẠI)";
                var lstqlnd = db.tbl_member.FirstOrDefault(n => n.Id == MaSua);
                lstqlnd.MatKhau = null;
                return View(lstqlnd);
            }

        }
    }
}
