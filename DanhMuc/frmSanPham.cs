using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BanHangLuuNiem.DanhMuc
{
    public partial class frmSanPham : Form
    {
        // Khởi tạo đối tượng xử lý CSDL và tiện ích
        Classes.DataProcesser dtBase = new Classes.DataProcesser();
        Classes.Function ft = new Classes.Function();
        string fileAnh = "";

        // Ánh xạ linh hoạt theo cả tên tùy chỉnh và tên mặc định của Visual Studio
        DataGridView DgvHang => this.Controls.Find("dgvHang", true).FirstOrDefault() as DataGridView
                             ?? this.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;

        PictureBox PicAnh => this.Controls.Find("picAnh", true).FirstOrDefault() as PictureBox
                          ?? this.Controls.Find("pictureBox1", true).FirstOrDefault() as PictureBox;

        ComboBox CboChatLieu => this.Controls.Find("cboChatLieu", true).FirstOrDefault() as ComboBox
                             ?? this.Controls.Find("comboBox1", true).FirstOrDefault() as ComboBox;

        TextBox TxtMaHang => this.Controls.Find("txtMaHang", true).FirstOrDefault() as TextBox
                          ?? this.Controls.Find("textBox1", true).FirstOrDefault() as TextBox;

        TextBox TxtTenHang => this.Controls.Find("txtTenHang", true).FirstOrDefault() as TextBox
                           ?? this.Controls.Find("textBox2", true).FirstOrDefault() as TextBox;

        TextBox TxtSoLuong => this.Controls.Find("txtSoLuong", true).FirstOrDefault() as TextBox
                           ?? this.Controls.Find("textBox3", true).FirstOrDefault() as TextBox;

        TextBox TxtDonGiaNhap => this.Controls.Find("txtDonGiaNhap", true).FirstOrDefault() as TextBox
                              ?? this.Controls.Find("textBox4", true).FirstOrDefault() as TextBox;

        TextBox TxtDonGiaBan => this.Controls.Find("txtDonGiaBan", true).FirstOrDefault() as TextBox
                             ?? this.Controls.Find("textBox5", true).FirstOrDefault() as TextBox;

        TextBox TxtGhiChu => this.Controls.Find("txtGhiChu", true).FirstOrDefault() as TextBox
                          ?? this.Controls.Find("textBox6", true).FirstOrDefault() as TextBox;

        // Các nút bấm
        Button BtnThemMoi => this.Controls.Find("btnThemMoi", true).FirstOrDefault() as Button
                          ?? this.Controls.Find("button1", true).FirstOrDefault() as Button;

        Button BtnLuu => this.Controls.Find("btnLuu", true).FirstOrDefault() as Button
                      ?? this.Controls.Find("button2", true).FirstOrDefault() as Button;

        Button BtnSua => this.Controls.Find("btnSua", true).FirstOrDefault() as Button
                      ?? this.Controls.Find("button3", true).FirstOrDefault() as Button;

        Button BtnXoa => this.Controls.Find("btnXoa", true).FirstOrDefault() as Button
                      ?? this.Controls.Find("button4", true).FirstOrDefault() as Button;

        Button BtnBoQua => this.Controls.Find("btnBoQua", true).FirstOrDefault() as Button
                        ?? this.Controls.Find("button5", true).FirstOrDefault() as Button;

        Button BtnThoat => this.Controls.Find("btnThoat", true).FirstOrDefault() as Button
                        ?? this.Controls.Find("button6", true).FirstOrDefault() as Button;

        public frmSanPham()
        {
            InitializeComponent();
        }

        // Xóa trắng dữ liệu các ô nhập
        void ResetValue()
        {
            if (TxtMaHang != null) TxtMaHang.Text = "";
            if (TxtTenHang != null) TxtTenHang.Text = "";
            if (CboChatLieu != null) CboChatLieu.Text = "";
            if (TxtSoLuong != null) TxtSoLuong.Text = "";
            if (TxtDonGiaBan != null) TxtDonGiaBan.Text = "";
            if (TxtDonGiaNhap != null) TxtDonGiaNhap.Text = "";
            if (PicAnh != null) PicAnh.Image = null;
            if (TxtGhiChu != null) TxtGhiChu.Text = "";
            fileAnh = "";
            if (TxtMaHang != null) TxtMaHang.Focus();
        }

        // 1. Khi Form Load
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            // Lấy dữ liệu bảng Chất liệu đổ vào cboChatLieu
            DataTable dtChatLieu = dtBase.DocBang("Select * from tblChatLieu");
            if (CboChatLieu != null)
            {
                ft.FillComBox(CboChatLieu, dtChatLieu, "TenChatLieu", "MaChatLieu");
                CboChatLieu.SelectedIndex = -1;
            }

            // Load dữ liệu lên dgvHang
            DataTable dtHang = dtBase.DocBang("select * from tblHang");
            if (DgvHang != null)
            {
                DgvHang.DataSource = dtHang;
                if (DgvHang.Columns.Count >= 7)
                {
                    DgvHang.Columns[0].HeaderText = "Mã hàng";
                    DgvHang.Columns[1].HeaderText = "Tên hàng";
                    DgvHang.Columns[2].HeaderText = "Mã CL";
                    DgvHang.Columns[3].HeaderText = "Số lượng";
                    DgvHang.Columns[4].HeaderText = "Giá nhập";
                    DgvHang.Columns[5].HeaderText = "Giá bán";
                    DgvHang.Columns[6].HeaderText = "File ảnh";

                    DgvHang.Columns[0].Width = 150;
                    DgvHang.Columns[1].Width = 250;
                    DgvHang.Columns[2].Width = 150;
                    DgvHang.Columns[3].Width = 150;
                    DgvHang.Columns[4].Width = 150;
                    DgvHang.Columns[5].Width = 150;
                    DgvHang.Columns[6].Width = 150;
                }
                if (DgvHang.Columns.Count > 7)
                {
                    DgvHang.Columns[7].HeaderText = "Ghi chú";
                }

                DgvHang.BackgroundColor = Color.LightBlue;
                DgvHang.CellClick -= dgvHang_CellClick;
                DgvHang.CellClick += dgvHang_CellClick;
            }
            dtHang.Dispose();

            // Cấm click các nút Sửa, Xóa, Lưu và Bỏ qua
            if (BtnSua != null) BtnSua.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = false;
            if (BtnBoQua != null) BtnBoQua.Enabled = false;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = true;
        }

        // 2. Click dòng trên DataGridView
        public void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvHang == null || DgvHang.CurrentRow == null || DgvHang.CurrentRow.Index < 0) return;

            if (BtnXoa != null) BtnXoa.Enabled = true;
            if (BtnSua != null) BtnSua.Enabled = true;
            if (BtnBoQua != null) BtnBoQua.Enabled = true;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = false;

            if (TxtMaHang != null) TxtMaHang.Text = DgvHang.CurrentRow.Cells[0].Value?.ToString();
            if (TxtTenHang != null) TxtTenHang.Text = DgvHang.CurrentRow.Cells[1].Value?.ToString();
            if (CboChatLieu != null) CboChatLieu.SelectedValue = DgvHang.CurrentRow.Cells[2].Value?.ToString();
            if (TxtSoLuong != null) TxtSoLuong.Text = DgvHang.CurrentRow.Cells[3].Value?.ToString();
            if (TxtDonGiaNhap != null) TxtDonGiaNhap.Text = DgvHang.CurrentRow.Cells[4].Value?.ToString();
            if (TxtDonGiaBan != null) TxtDonGiaBan.Text = DgvHang.CurrentRow.Cells[5].Value?.ToString();
            fileAnh = DgvHang.CurrentRow.Cells[6].Value?.ToString();

            if (TxtGhiChu != null && DgvHang.CurrentRow.Cells.Count > 7 && DgvHang.CurrentRow.Cells[7].Value != null)
            {
                TxtGhiChu.Text = DgvHang.CurrentRow.Cells[7].Value.ToString();
            }

            try
            {
                if (PicAnh != null) PicAnh.Image = Image.FromFile(Application.StartupPath + "\\Images\\Hang\\" + fileAnh);
            }
            catch
            {
                if (PicAnh != null) PicAnh.Image = null;
            }
        }

        // 3. Nút Thêm mới
        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            if (BtnSua != null) BtnSua.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = true;
            if (BtnLuu != null) BtnLuu.Enabled = true;
            if (BtnBoQua != null) BtnBoQua.Enabled = true;

            ResetValue();
        }

        // 4. Nút Lưu (Thêm mới hoặc Cập nhật)
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (TxtMaHang != null && TxtMaHang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập mã hàng");
                TxtMaHang.Focus();
                return;
            }
            if (TxtTenHang != null && TxtTenHang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập tên hàng");
                TxtTenHang.Focus();
                return;
            }
            if (CboChatLieu != null && CboChatLieu.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải chọn chất liệu");
                return;
            }
            if (TxtSoLuong != null && TxtSoLuong.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập số lượng");
                TxtSoLuong.Focus();
                return;
            }
            if (TxtDonGiaNhap != null && TxtDonGiaNhap.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đơn giá nhập");
                TxtDonGiaNhap.Focus();
                return;
            }

            // A. Thêm mới
            if (BtnThemMoi != null && BtnThemMoi.Enabled == true && (BtnSua == null || BtnSua.Enabled == false))
            {
                DataTable dtSP = dtBase.DocBang("Select * from tblHang where MaHang='" + TxtMaHang.Text + "'");
                if (dtSP.Rows.Count > 0)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại. Mời bạn nhập mã khác!!!");
                    TxtMaHang.Focus();
                    return;
                }

                string strInsert = "Insert into tblHang values('" + TxtMaHang.Text + "',N'" + TxtTenHang.Text + "','"
                    + CboChatLieu.SelectedValue + "'," + int.Parse(TxtSoLuong.Text)
                    + "," + float.Parse(TxtDonGiaNhap.Text) + "," + float.Parse(TxtDonGiaBan.Text)
                    + ",'" + fileAnh + "',N'" + (TxtGhiChu != null ? TxtGhiChu.Text : "") + "')";
                dtBase.CapNhatDuLieu(strInsert);
            }

            // B. Sửa dữ liệu
            if (BtnSua != null && BtnSua.Enabled == true)
            {
                string sqlUpdate = "update tblHang set TenHang=N'" + TxtTenHang.Text + "'"
                    + ",MaChatLieu='" + CboChatLieu.SelectedValue + "'"
                    + ",SoLuong=" + Convert.ToInt32(TxtSoLuong.Text)
                    + ",DonGiaNhap=" + float.Parse(TxtDonGiaNhap.Text)
                    + ",DonGiaBan=" + float.Parse(TxtDonGiaBan.Text)
                    + ",Anh='" + fileAnh + "'"
                    + ",GhiChu=N'" + (TxtGhiChu != null ? TxtGhiChu.Text : "") + "' where MaHang='" + TxtMaHang.Text + "'";
                dtBase.CapNhatDuLieu(sqlUpdate);
            }

            // Cập nhật lại GridView
            if (DgvHang != null)
            {
                DataTable dtHang = dtBase.DocBang("select * from tblHang");
                DgvHang.DataSource = dtHang;
            }

            // Đưa form về trạng thái ban đầu
            if (BtnSua != null) BtnSua.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = false;
            if (BtnBoQua != null) BtnBoQua.Enabled = false;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = true;

            ResetValue();
        }

        // 5. Nút Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (BtnThemMoi != null) BtnThemMoi.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = true;
            if (BtnBoQua != null) BtnBoQua.Enabled = true;
        }

        // 6. Nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Xóa mặt hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtBase.CapNhatDuLieu("Delete tblHang where MaHang='" + TxtMaHang.Text + "'");
                if (DgvHang != null) DgvHang.DataSource = dtBase.DocBang("select * from tblHang");
            }

            if (BtnSua != null) BtnSua.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = false;
            if (BtnBoQua != null) BtnBoQua.Enabled = false;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = true;

            ResetValue();
        }

        // 7. Nút Bỏ qua
        private void btnBoQua_Click(object sender, EventArgs e)
        {
            if (BtnSua != null) BtnSua.Enabled = false;
            if (BtnXoa != null) BtnXoa.Enabled = false;
            if (BtnLuu != null) BtnLuu.Enabled = false;
            if (BtnBoQua != null) BtnBoQua.Enabled = false;
            if (BtnThemMoi != null) BtnThemMoi.Enabled = true;

            ResetValue();
        }

        // 8. Nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // 9. Nút chọn Ảnh
        private void btnAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgAnh = new OpenFileDialog();
            dlgAnh.Filter = "Bitmap(*.bmp)|*.bmp|Gif(*.gif)|*.gif|All files(*.*)|*.*";
            dlgAnh.InitialDirectory = Application.StartupPath;
            dlgAnh.FilterIndex = 3;
            dlgAnh.Title = "Chọn ảnh để hiển thị";

            if (dlgAnh.ShowDialog() == DialogResult.OK)
            {
                if (PicAnh != null) PicAnh.Image = Image.FromFile(dlgAnh.FileName);
                string[] str = dlgAnh.FileName.Split('\\');
                fileAnh = str[str.Length - 1].ToString();
            }
        }

        // 10. Nút Tìm kiếm
        private void btnTim_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM tblHang WHERE 1=1";
            if (TxtMaHang != null && TxtMaHang.Text.Trim() != "")
                sql += " AND MaHang LIKE '%" + TxtMaHang.Text.Trim() + "%'";
            if (TxtTenHang != null && TxtTenHang.Text.Trim() != "")
                sql += " AND TenHang LIKE N'%" + TxtTenHang.Text.Trim() + "%'";
            if (CboChatLieu != null && CboChatLieu.SelectedValue != null && CboChatLieu.Text != "")
                sql += " AND MaChatLieu = '" + CboChatLieu.SelectedValue.ToString() + "'";

            DataTable dtSearch = dtBase.DocBang(sql);
            if (dtSearch.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (DgvHang != null) DgvHang.DataSource = dtSearch;
        }

        // 11. Nút Xuất ra Excel
        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (DgvHang == null || DgvHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "DanhMucHangHoa.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                string[] columnNames = DgvHang.Columns.Cast<DataGridViewColumn>().Select(column => "\"" + column.HeaderText + "\"").ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataGridViewRow row in DgvHang.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string[] fields = row.Cells.Cast<DataGridViewCell>().Select(cell => "\"" + (cell.Value?.ToString().Replace("\"", "\"\"") ?? "") + "\"").ToArray();
                        sb.AppendLine(string.Join(",", fields));
                    }
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtGhiChu_TextChanged(object sender, EventArgs e)
        {
        }

        // Ánh xạ sang các hàm sự kiện nếu form đang nối với tên button mặc định
        private void button1_Click(object sender, EventArgs e) => btnThemMoi_Click(sender, e);
        private void button2_Click(object sender, EventArgs e) => btnLuu_Click(sender, e);
        private void button3_Click(object sender, EventArgs e) => btnSua_Click(sender, e);
        private void button4_Click(object sender, EventArgs e) => btnXoa_Click(sender, e);
        private void button5_Click(object sender, EventArgs e) => btnBoQua_Click(sender, e);
        private void button6_Click(object sender, EventArgs e) => btnThoat_Click(sender, e);
    }
}