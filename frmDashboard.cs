using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void visitWeb_Click(object sender, EventArgs e)
        {
            bmBrowser.Navigate("https://bloggingmetrics.com/");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.Clear();

                frmLogin login = new frmLogin();
                login.Show();

                this.Close();
            }
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
        }

        private void bmBrowser_DocumentCompleted(
            object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnManageEmployees_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            Form1 employeeForm = new Form1();
            employeeForm.ShowDialog();
        }
    }
}