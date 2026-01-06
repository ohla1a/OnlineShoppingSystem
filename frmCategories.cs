using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        void LoadCategories()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Categories", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCategories.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Enter a category name", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Categories (CategoryName) VALUES (@n)", con))
                {
                    cmd.Parameters.AddWithValue("@n", txtCategoryName.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            LoadCategories();
            txtCategoryName.Clear();
            txtCategoryName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow == null) return;

            if (MessageBox.Show("Delete selected category?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvCategories.CurrentRow.Cells["CategoryID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE CategoryID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadCategories();
        }
    }
}
