using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        void LoadCustomers()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
                dgvCustomers.ClearSelection();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter Full Name.");
                return;
            }

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "INSERT INTO Customers (FullName, Email, Phone, Address) VALUES (@n,@e,@p,@a)";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@n", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            LoadCustomers();
            ClearInputs();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "UPDATE Customers SET FullName=@n, Email=@e, Phone=@p, Address=@a WHERE CustomerID=@id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@n", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadCustomers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);
            var ans = MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo);
            if (ans != DialogResult.Yes) return;

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                string q = "DELETE FROM Customers WHERE CustomerID=@id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadCustomers();
            ClearInputs();
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;
            txtFullName.Text = dgvCustomers.CurrentRow.Cells["FullName"].Value?.ToString();
            txtEmail.Text = dgvCustomers.CurrentRow.Cells["Email"].Value?.ToString();
            txtPhone.Text = dgvCustomers.CurrentRow.Cells["Phone"].Value?.ToString();
            txtAddress.Text = dgvCustomers.CurrentRow.Cells["Address"].Value?.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        void ClearInputs()
        {
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            dgvCustomers.ClearSelection();
        }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtPhone_TextChanged(object sender, EventArgs e) { }
        private void txtAddress_TextChanged(object sender, EventArgs e) { }

        private void dgvCustomers_SelectionChanged_1(object sender, EventArgs e)
        {

        }

        private void CusLB_Click(object sender, EventArgs e)
        {

        }
    }
}
