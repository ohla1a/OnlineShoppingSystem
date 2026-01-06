using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmStaff : Form
    {
        public frmStaff() { InitializeComponent(); }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            LoadStaff();
        }

        void LoadStaff()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Staff", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStaff.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Staff (FullName, Role, Email) VALUES (@n,@r,@e)", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@r", txtRole.Text);
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadStaff();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStaff.CurrentRow == null) return;
            if (MessageBox.Show("Delete selected staff member?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvStaff.CurrentRow.Cells["StaffID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Staff WHERE StaffID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadStaff();
        }

        // Optional: populate textboxes when grid selection changes
        private void dgvStaff_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStaff.CurrentRow == null)
            {
                txtFullName.Clear();
                txtRole.Clear();
                txtEmail.Clear();
                return;
            }

            var row = dgvStaff.CurrentRow;
            txtFullName.Text = row.Cells["FullName"].Value?.ToString() ?? string.Empty;
            txtRole.Text = row.Cells["Role"].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
        }
    }
}
