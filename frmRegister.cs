using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" ||
                txtPassword.Text == "" ||
                txtConPassword.Text == "")
            {
                MessageBox.Show(
                    "Username and password fields cannot be empty.",
                    "Register Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (txtPassword.Text != txtConPassword.Text)
            {
                MessageBox.Show(
                    "Passwords do not match, please re-enter.",
                    "Register Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtPassword.Text = "";
                txtConPassword.Text = "";
                txtPassword.Focus();

                return;
            }

            try
            {
                User user = new User();

                // Check whether the username already exists.
                if (user.UsernameExists(txtUsername.Text.Trim()))
                {
                    MessageBox.Show(
                        "That username is already taken.",
                        "Register Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtUsername.Focus();
                    return;
                }

                // Register the new user.
                if (user.RegisterUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text))
                {
                    MessageBox.Show(
                        "Your account has been successfully created.",
                        "Registration Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtConPassword.Text = "";
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
                txtConPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
                txtConPassword.PasswordChar = '•';
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConPassword.Text = "";
            txtUsername.Focus();
        }

        private void clickLogin_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            Application.Exit();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}