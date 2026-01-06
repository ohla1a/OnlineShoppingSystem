using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    public partial class frmDashboard : Form
    {
        // 1. WINDOW DRAGGING LOGIC (Because FormBorderStyle = None)
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private Form activeForm = null;

        public frmDashboard()
        {
            InitializeComponent();
            OpenChildForm(new frmHome());
        }

        // 2. THE MAGIC FUNCTION: Opens forms inside the panel
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close(); // Close the previous form

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // "pnlContent" is the name of your big cream panel on the right
            // If yours is named "panel2" or something else, rename it to "pnlContent" in properties!
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // 3. ALLOW DRAGGING THE WINDOW
        // Link this to your Top Pink Panel's "MouseDown" event
        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // 4. BUTTON CLICKS - CONNECTING YOUR FORMS
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmCustomers());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmProducts());
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmOrder());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmPayments());
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmCategories());
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmSuppliers());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmStaff());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmHome());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }
}