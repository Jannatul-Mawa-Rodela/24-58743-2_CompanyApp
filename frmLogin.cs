using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                User user = new User();

                int userID = user.ValidateLogin(
                    txtUsername.Text.Trim(),
                    txtPassword.Text);

                if (userID > 0)
                {
                    Session.UserID = userID;
                    Session.Username = txtUsername.Text.Trim();

                    frmDashboard dashboard = new frmDashboard();

                    this.Hide();
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show(
                        "Wrong username or password, please try again.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database error:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void clickRegister_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}