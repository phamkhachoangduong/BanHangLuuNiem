using System;
using System.Data;
using System.Data.SqlClient;

namespace BanHangLuuNiem.Classes
{
    // Đặt tên lớp là DataProcesser, đảm bảo DocBang và CapNhatDuLieu đều là public
    public class DataProcesser
    {
        // Chuỗi kết nối CSDL SQL Server (sửa lại tên Server nếu máy bạn không dùng .\SQLEXPRESS)
        string strConnect = "Data Source=.\\SQLEXPRESS;DataBase=DuLieu;Integrated Security=True";
        SqlConnection sqlConnect = null;

        // Hàm mở kết nối
        private void KetNoiCSDL()
        {
            sqlConnect = new SqlConnection(strConnect);
            if (sqlConnect.State != ConnectionState.Open)
            {
                sqlConnect.Open();
            }
        }

        // Hàm đóng kết nối
        private void DongKetNoiCSDL()
        {
            if (sqlConnect != null && sqlConnect.State != ConnectionState.Closed)
            {
                sqlConnect.Close();
                sqlConnect.Dispose();
            }
        }

        // Hàm thực thi câu lệnh dạng Select trả về một DataTable
        public DataTable DocBang(string sql)
        {
            DataTable dtBang = new DataTable();
            KetNoiCSDL();
            SqlDataAdapter sqldataAdapte = new SqlDataAdapter(sql, sqlConnect);
            sqldataAdapte.Fill(dtBang);
            DongKetNoiCSDL();
            return dtBang;
        }

        // Hàm thực lệnh insert hoặc update hoặc delete
        public void CapNhatDuLieu(string sql)
        {
            KetNoiCSDL();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnect;
            sqlCommand.CommandText = sql;
            sqlCommand.ExecuteNonQuery();
            DongKetNoiCSDL();
            sqlCommand.Dispose();
        }
    }
}