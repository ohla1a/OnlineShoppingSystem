using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmSuppliers : Form
    {
        public frmSuppliers() { InitializeComponent(); }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        void LoadSuppliers()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Suppliers", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSuppliers.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Suppliers (SupplierName, Email, Contact) VALUES (@n,@e,@p)", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtSupplierName.Text);
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadSuppliers();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvSuppliers.CurrentRow.Cells["SupplierID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Suppliers SET SupplierName=@n, Email=@e, Contact=@p WHERE SupplierID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtSupplierName.Text);
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadSuppliers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvSuppliers.CurrentRow.Cells["SupplierID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Suppliers WHERE SupplierID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadSuppliers();
        }

        // Optional: populate textboxes when grid selection changes
        private void dgvSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null)
            {
                txtSupplierName.Clear();
                txtEmail.Clear();
                txtPhone.Clear();
                return;
            }

            var row = dgvSuppliers.CurrentRow;
            txtSupplierName.Text = row.Cells["SupplierName"].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
            txtPhone.Text = row.Cells["Contact"].Value?.ToString() ?? string.Empty;
        }
    }
}
