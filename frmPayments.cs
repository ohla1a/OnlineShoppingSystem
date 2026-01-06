using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmPayments : Form
    {
        public frmPayments()
        {
            InitializeComponent();

            // wire order selection so amount is auto-filled
            if (cboOrder != null)
            {
                cboOrder.SelectedIndexChanged -= cboOrder_SelectedIndexChanged;
                cboOrder.SelectedIndexChanged += cboOrder_SelectedIndexChanged;
            }
        }

        private void frmPayments_Load(object sender, EventArgs e)
        {
            LoadOrders();
            LoadPayments();

            // populate amount for initial selection
            if (cboOrder.SelectedValue != null)
                TryLoadSelectedOrderAmount();
        }

        void LoadOrders()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT OrderID FROM Orders", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboOrder.DisplayMember = "OrderID";
                cboOrder.ValueMember = "OrderID";
                cboOrder.DataSource = dt;
            }
        }

        void LoadPayments()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Payments", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPayments.DataSource = dt;
            }
        }

        private void cboOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryLoadSelectedOrderAmount();
        }

        private void TryLoadSelectedOrderAmount()
        {
            if (cboOrder.SelectedValue == null) { txtAmount.Text = "0.00"; return; }

            int orderId;
            if (!int.TryParse(cboOrder.SelectedValue.ToString(), out orderId))
            {
                txtAmount.Text = "0.00";
                return;
            }

            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT TotalAmount FROM Orders WHERE OrderID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", orderId);
                con.Open();
                var val = cmd.ExecuteScalar();
                con.Close();

                if (val != null && decimal.TryParse(val.ToString(), out decimal total))
                    txtAmount.Text = total.ToString("0.00");
                else
                    txtAmount.Text = "0.00";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboMethod.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a payment method.");
                return;
            }

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Payments (OrderID, PaymentDate, PaymentMethod, Amount) VALUES (@o,@d,@m,@a)", con);
                cmd.Parameters.AddWithValue("@o", cboOrder.SelectedValue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@d", dtPaymentDate.Value);
                cmd.Parameters.AddWithValue("@m", cboMethod.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@a", decimal.TryParse(txtAmount.Text, out decimal amt) ? (object)amt : txtAmount.Text);
                cmd.ExecuteNonQuery();
            }
            LoadPayments();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPayments.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvPayments.CurrentRow.Cells["PaymentID"].Value);
            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Payments WHERE PaymentID=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            LoadPayments();
        }
    }
}
