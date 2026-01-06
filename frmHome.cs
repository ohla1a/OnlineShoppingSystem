using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineShoppingDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();

                    // 1. Get Total Orders
                    string sqlOrders = "SELECT COUNT(*) FROM Orders";
                    SqlCommand cmd1 = new SqlCommand(sqlOrders, con);
                    lblTotalOrders.Text = cmd1.ExecuteScalar().ToString();

                    // 2. Get Total Sales (Handle null if no sales yet)
                    string sqlSales = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders";
                    SqlCommand cmd2 = new SqlCommand(sqlSales, con);
                    decimal totalSales = Convert.ToDecimal(cmd2.ExecuteScalar());
                    lblTotalSales.Text = "฿" + totalSales.ToString("N2"); // Formats as money

                    // 3. Get Total Customers
                    string sqlCust = "SELECT COUNT(*) FROM Customers";
                    SqlCommand cmd3 = new SqlCommand(sqlCust, con);
                    lblTotalCustomers.Text = cmd3.ExecuteScalar().ToString();

                    // 4. Get Total Products
                    string sqlProd = "SELECT COUNT(*) FROM Products";
                    SqlCommand cmd4 = new SqlCommand(sqlProd, con);
                    lblTotalProducts.Text = cmd4.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading stats: " + ex.Message);
                }
            }
        }

    }
}