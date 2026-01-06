using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmOrder : Form
    {
        // SQL connection
        SqlConnection con = new SqlConnection(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineShoppingDB;Integrated Security=True");


        public frmOrder()
        {
            InitializeComponent();

            // Ensure event handlers are wired in case Designer missed them
            if (nudQty != null)
            {
                nudQty.ValueChanged -= nudQty_ValueChanged;
                nudQty.ValueChanged += nudQty_ValueChanged;
            }

            if (cboProduct != null)
            {
                cboProduct.SelectedIndexChanged -= cboProduct_SelectedIndexChanged;
                cboProduct.SelectedIndexChanged += cboProduct_SelectedIndexChanged;
            }

            // Make sure initial subtotal reflects current price/qty
            UpdateSubtotal();
        }

        // Existing load method kept for logic
        private void frmOrder_Load(object sender, EventArgs e)
        {
            LoadOrderIDs();
            LoadCustomers();
            LoadProducts();
            txtTotal.Text = "0.00";
            txtPrice.Text = "0.00";
            txtSubtotal.Text = "0.00";

            // load saved orders into the new grid
            LoadSavedOrders();
        }

        // New wire method requested: forwards to existing load logic
        private void frmOrders_Load(object sender, EventArgs e)
        {
            // forward to the actual initializer to avoid duplicating logic
            frmOrder_Load(sender, e);
        }

        void LoadCustomers()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT CustomerID, FullName FROM Customers", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboCustomer.DisplayMember = "FullName";
                cboCustomer.ValueMember = "CustomerID";
                cboCustomer.DataSource = dt;
            }
        }

        void LoadProducts()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT ProductID, ProductName, Price FROM Products", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboProduct.DisplayMember = "ProductName";
                cboProduct.ValueMember = "ProductID";
                cboProduct.DataSource = dt;
            }
        }

        // new: load saved orders list into the right-hand grid
        void LoadSavedOrders()
        {
            if (dgvSavedOrders == null) return;

            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orders ORDER BY OrderDate DESC", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSavedOrders.DataSource = dt;
            }
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue == null) return;
            int pid;
            if (!int.TryParse(cboProduct.SelectedValue.ToString(), out pid)) return;
            using (SqlConnection con = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT Price FROM Products WHERE ProductID=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", pid);
                con.Open();
                var val = cmd.ExecuteScalar();
                con.Close();
                if (val != null && decimal.TryParse(val.ToString(), out decimal price))
                    txtPrice.Text = price.ToString("0.00");
                else
                    txtPrice.Text = "0.00";
                UpdateSubtotal();
            }
        }

        private void nudQty_ValueChanged(object sender, EventArgs e)
        {
            UpdateSubtotal();
        }

        void UpdateSubtotal()
        {
            decimal price = 0;
            decimal.TryParse(txtPrice.Text, out price);
            int qty = (int)nudQty.Value;
            txtSubtotal.Text = (price * qty).ToString("0.00");
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue == null) return;
            int pid = Convert.ToInt32(cboProduct.SelectedValue);
            string pname = cboProduct.Text;
            decimal price = 0;
            decimal.TryParse(txtPrice.Text, out price);
            int qty = (int)nudQty.Value;
            decimal subtotal = price * qty;

            DataTable dt;
            if (dgvOrderItems.DataSource == null)
            {
                dt = new DataTable();
                dt.Columns.Add("ProductID", typeof(int));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Subtotal", typeof(decimal));
                dgvOrderItems.DataSource = dt;
            }
            else dt = (DataTable)dgvOrderItems.DataSource;

            // combine same product
            DataRow existing = null;
            foreach (DataRow r in dt.Rows)
                if ((int)r["ProductID"] == pid) { existing = r; break; }

            if (existing != null)
            {
                existing["Quantity"] = (int)existing["Quantity"] + qty;
                existing["Subtotal"] = (decimal)existing["Quantity"] * (decimal)existing["Price"];
            }
            else
            {
                dt.Rows.Add(pid, pname, price, qty, subtotal);
            }

            UpdateTotal();
        }

        void UpdateTotal()
        {
            decimal total = 0;
            if (dgvOrderItems.DataSource != null)
            {
                DataTable dt = (DataTable)dgvOrderItems.DataSource;
                foreach (DataRow r in dt.Rows) total += (decimal)r["Subtotal"];
            }
            txtTotal.Text = total.ToString("0.00");
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrderItems.DataSource == null) { MessageBox.Show("Add items first"); return; }
            if (cboCustomer.SelectedValue == null) { MessageBox.Show("Select a customer"); return; }

            int customerId = Convert.ToInt32(cboCustomer.SelectedValue);
            int staffId = 1; // default staff; later you can add cboStaff
            decimal totalAmount = decimal.Parse(txtTotal.Text);

            using (SqlConnection con = DBConnection.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Orders (CustomerID, StaffID, OrderDate, TotalAmount) OUTPUT INSERTED.OrderID VALUES (@c,@s,GETDATE(),@t)", con, tran);
                    cmd.Parameters.AddWithValue("@c", customerId);
                    cmd.Parameters.AddWithValue("@s", staffId);
                    cmd.Parameters.AddWithValue("@t", totalAmount);
                    int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                    DataTable dt = (DataTable)dgvOrderItems.DataSource;
                    foreach (DataRow r in dt.Rows)
                    {
                        SqlCommand od = new SqlCommand("INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Subtotal) VALUES (@o,@p,@q,@s)", con, tran);
                        od.Parameters.AddWithValue("@o", orderId);
                        od.Parameters.AddWithValue("@p", (int)r["ProductID"]);
                        od.Parameters.AddWithValue("@q", (int)r["Quantity"]);
                        od.Parameters.AddWithValue("@s", (decimal)r["Subtotal"]);
                        od.ExecuteNonQuery();

                        // optional reduce stock
                        SqlCommand upd = new SqlCommand("UPDATE Products SET Stock = Stock - @q WHERE ProductID = @p", con, tran);
                        upd.Parameters.AddWithValue("@q", (int)r["Quantity"]);
                        upd.Parameters.AddWithValue("@p", (int)r["ProductID"]);
                        upd.ExecuteNonQuery();
                    }

                    tran.Commit();
                    MessageBox.Show("Order saved. OrderID: " + orderId);
                    // clear
                    dgvOrderItems.DataSource = null;
                    txtTotal.Text = "0.00";
                    txtPrice.Text = "0.00";
                    txtSubtotal.Text = "0.00";

                    // refresh saved orders grid
                    LoadSavedOrders();
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            // 1. Check if they selected anything
            if (cboOrderID.SelectedIndex == -1) // -1 means nothing selected
            {
                MessageBox.Show("Please select an Order ID from the list.", "Select Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Get the number they picked
            int selectedId = Convert.ToInt32(cboOrderID.SelectedItem);

            // 3. Open the report
            try
            {
                frmReportViewer reportForm = new frmReportViewer(selectedId);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening report: " + ex.Message);
            }
        }

        // Call this function to fill the dropdown list
        private void LoadOrderIDs()
        {
            cboOrderID.Items.Clear(); // Clear old list

            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineShoppingDB;Integrated Security=True";

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    // Get all IDs, newest first
                    string sql = "SELECT OrderID FROM Orders ORDER BY OrderID DESC";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, con))
                    {
                        using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Add every ID found to the ComboBox
                                cboOrderID.Items.Add(reader["OrderID"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Order IDs: " + ex.Message);
                }
            }
        }
    }
}
