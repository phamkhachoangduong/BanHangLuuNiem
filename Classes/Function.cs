using System;
using System.Data;
using System.Windows.Forms;

namespace BanHangLuuNiem.Classes
{
    public class Function
    {
        // Hàm đổ dữ liệu vào ComboBox
        public void FillComBox(ComboBox cb, DataTable dt, string display, string value)
        {
            cb.DataSource = dt;
            cb.DisplayMember = display;
            cb.ValueMember = value;
        }
    }
}