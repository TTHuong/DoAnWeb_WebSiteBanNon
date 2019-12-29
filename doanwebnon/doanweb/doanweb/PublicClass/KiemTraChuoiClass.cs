using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace doanweb.PublicClass
{
    public class KiemTraChuoiClass
    {
        //hàm này mục đích là dùng để cắt 1 chuỗi các ký tự ra từng ký tự nhỏ và kiểm tra xem có các ký tự đặc biệt mà website cấm 
        //hay không , nếu có trả về false , còn không có trả về true
        public bool hamcatchuoi(string chuoi)
        {
            for (int i = 0; i < chuoi.Length; i++)
            {
                if (chuoi[i] == '`' || chuoi[i] == '~' || chuoi[i] == '!' || chuoi[i] == '@' || chuoi[i] == '#' || chuoi[i] == '$' || chuoi[i] == '%' || chuoi[i] == '^' || chuoi[i] == '&' || chuoi[i] == '/' || chuoi[i] == '*' || chuoi[i] == '-' || chuoi[i] == '+' || chuoi[i] == '(' || chuoi[i] == ')' || chuoi[i] == '_' || chuoi[i] == '=' || chuoi[i] == '{' || chuoi[i] == '}' || chuoi[i] == '[' || chuoi[i] == ']' || chuoi[i] == ';' || chuoi[i] == ':' || chuoi[i] == '"' || chuoi[i] == '|' || chuoi[i] == '<' || chuoi[i] == ',' || chuoi[i] == '>' || chuoi[i] == '.' || chuoi[i] == '?' || chuoi[i] == '/')
                {
                    return false;
                }
            }
            return true;
        }

        //hàm này dùng để kiểm tra chuỗi có ký tự số hay không,nếu có trả về true ngược lại trả về false
        public bool kiemtraso(Char so)
        {
            bool x = false;
            if (so == '1' || so == '2' || so == '3' || so == '4' || so == '5' || so == '7' || so == '6' || so == '8' || so == '9' || so == '0')
            {
                x = true;
            }
            return x;
        }


        //hàm này dùng để kiểm tra độc mạnh của mật khẩu xem coi mật khẩu có đủ độ mạnh chưa, mật khẩu mạnh là mật khẩu có độ dài lớn hơn
        // 8 ký tự và có ít nhất 1 ký tự đặc biệt và ít nhất 1 ký tự thường và ít nhất 1 ký tự hoa và ít nhất 1 ký tự số , nếu chuỗi
        //là mật khẩu mạnh thì sẽ trả về true  ngược lại sẽ trả về false
        public bool hamkiemtradomanhmatkhau(string chuoi)
        {
            int ktdb = 0, ktt = 0, kth = 0, kts = 0;
            bool ketqua = false;
            if (chuoi.Length < 8)
            {
                ketqua = false;
            }
            else
            {
                for (int i = 0; i < chuoi.Length; i++)
                {
                    if (chuoi[i] == '@' || chuoi[i] == '&' || chuoi[i] == '.' || chuoi[i] == ',' || chuoi[i] == ' ')
                    {
                        ktdb++;
                    }
                    else if (chuoi[i] == Char.ToLower(chuoi[i]))
                    {
                        ktt++;
                    }
                    else if (chuoi[i] == Char.ToUpper(chuoi[i]))
                    {
                        kth++;
                    }
                }
            }
            if (Regex.IsMatch(chuoi, @"\d") == true)
            {
                kts++;
            }

            if (ktdb >= 1 && ktt >= 1 && kth >= 1 && kts >= 1)
            {
                ketqua = true;
            }
            else
            {
                ketqua = false;
            }

            return ketqua;
        }

        public bool Kiemtraemail(string chuoi)
        {
            return Regex.IsMatch(chuoi, @"^[a-z][a-z0-9_\.]{5,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$");
        }
        public bool Kiemtrasdt(string chuoi)
        {
            return Regex.IsMatch(chuoi, @"\D");
        }

        public bool Kiemtradangso(String chuoi)
        {
            bool kq = false;
            double thu = 0;
            if(Double.TryParse(chuoi,out thu)==true)
            {
                kq = true;
            }
            return kq;
        }

    }
}