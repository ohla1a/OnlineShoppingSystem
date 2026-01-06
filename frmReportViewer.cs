using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmReportViewer : Form
    {
        // 1. Create a variable to hold the specific ID
        private int _PrintOrderID;

        // 2. Change the Constructor to ask for an ID
        public frmReportViewer(int orderID)
        {
            InitializeComponent();
            _PrintOrderID = orderID; // Save the ID to use later
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            LoadOrderReport();
        }

        private void LoadOrderReport()
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineShoppingDB;Integrated Security=True";

            // 3. UPDATE THE QUERY: Added "WHERE o.OrderID = @OID"
            string query = @"
                SELECT 
                    o.OrderID, 
                    c.FullName AS CustomerName, 
                    o.OrderDate, 
                    p.ProductName, 
                    od.Quantity, 
                    p.Price, 
                    (od.Quantity * p.Price) AS Subtotal,
                    o.TotalAmount
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
                INNER JOIN Products p ON od.ProductID = p.ProductID
                WHERE o.OrderID = @OID"; // <--- This filters for ONE order only

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // 4. Pass the ID securely to the database
                    cmd.Parameters.AddWithValue("@OID", _PrintOrderID);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Could not find details for Order #" + _PrintOrderID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Close if no data found
                return;
            }

            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.DataSources.Clear();

            // Make sure "dsOrders" matches your RDLC dataset name
            ReportDataSource rds = new ReportDataSource("dsOrders", dt);

            reportViewer1.LocalReport.ReportPath = "rptOrderList.rdlc";
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Set zoom to page width so it looks nice
            reportViewer1.ZoomMode = ZoomMode.PageWidth;
            reportViewer1.RefreshReport();
        }
    }
}