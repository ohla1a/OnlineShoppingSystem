using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadSuppliers();
            LoadProducts();
        }

        void LoadCategories()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT CategoryID, CategoryName FROM Categories", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryID";
                cboCategory.DataSource = dt;
            }
        }

        void LoadSuppliers()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT SupplierID, SupplierName FROM Suppliers", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboSupplier.DisplayMember = "SupplierName";
                cboSupplier.ValueMember = "SupplierID";
                cboSupplier.DataSource = dt;
            }
        }

        void LoadProducts()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProducts.DataSource = dt;
            }
        }

        void ClearInputs()
        {
            txtProductName.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            cboCategory.SelectedIndex = -1;
            cboSupplier.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "INSERT INTO Products (ProductName, CategoryID, SupplierID, Price, Stock) VALUES (@n,@c,@s,@p,@st)";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@n", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@c", cboCategory.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@s", cboSupplier.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@st", txtStock.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadProducts();
            ClearInputs();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "UPDATE Products SET ProductName=@n, CategoryID=@c, SupplierID=@s, Price=@p, Stock=@st WHERE ProductID=@id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@n", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@c", cboCategory.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@s", cboSupplier.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@st", txtStock.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "DELETE FROM Products WHERE ProductID=@id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadProducts();
            ClearInputs();
        }

        // Clear button click -> calls ClearInputs
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            txtProductName.Text = dgvProducts.CurrentRow.Cells["ProductName"].Value.ToString();
            txtPrice.Text = dgvProducts.CurrentRow.Cells["Price"].Value.ToString();
            txtStock.Text = dgvProducts.CurrentRow.Cells["Stock"].Value.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
