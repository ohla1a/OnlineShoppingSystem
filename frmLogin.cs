using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace OnlineShoppingManagementSystem
{
    public partial class frmLogin : Form
    {
        private Timer fadeTimer = new Timer();

        public frmLogin()
        {
            InitializeComponent();

            // fade-in effect
            this.Opacity = 0;
            fadeTimer.Interval = 20;
            fadeTimer.Tick += FadeIn;
            fadeTimer.Start();

            // load image from project folder
            try
            {
                leftImage.Image = Image.FromFile("login_image.png");
            }
            catch
            {
                // Image not found but form still works
            }
        }

        private void FadeIn(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.05;
            else
                fadeTimer.Stop();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(DBConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username=@u AND Password=@p", con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    frmDashboard dash = new frmDashboard();
                    dash.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
        }


        private void leftImage_Click(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

