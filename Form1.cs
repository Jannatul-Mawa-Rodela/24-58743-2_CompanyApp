using System;
using System.Windows.Forms;

namespace EmployeeDetails
{
    public partial class Form1 : Form
    {
        Employee employee = new Employee();

        public Form1()
        {
            InitializeComponent();
            dgvEmployeeDetails.DataSource = employee.GetEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            employee.EmpId = txtEmpId.Text.Trim();
            employee.EmpName = txtEmpName.Text.Trim();
            employee.Age = Convert.ToInt32(txtAge.Text);
            employee.ContactNo = txtContactNo.Text.Trim();
            employee.Gender = cboGender.Text;

            try
            {
                var success = employee.InsertEmployee(employee);

                if (success)
                {
                    MessageBox.Show(
                        "Employee has been added successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    dgvEmployeeDetails.DataSource = employee.GetEmployees();
                    ClearControls();
                }
                else
                {
                    MessageBox.Show(
                        "Error occurred. Please try again.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            employee.EmpId = txtEmpId.Text.Trim();
            employee.EmpName = txtEmpName.Text.Trim();
            employee.Age = Convert.ToInt32(txtAge.Text);
            employee.ContactNo = txtContactNo.Text.Trim();
            employee.Gender = cboGender.Text;

            try
            {
                var success = employee.UpdateEmployee(employee);

                if (success)
                {
                    MessageBox.Show(
                        "Employee has been updated successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    dgvEmployeeDetails.DataSource = employee.GetEmployees();
                    ClearControls();
                }
                else
                {
                    MessageBox.Show(
                        "Employee not found. Please try again.",
                        "Update Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtEmpId.Text.Trim() == "")
            {
                MessageBox.Show("Please enter or select an Employee ID.");
                return;
            }

            try
            {
                employee.EmpId = txtEmpId.Text.Trim();

                var success = employee.DeleteEmployee(employee);

                if (success)
                {
                    MessageBox.Show(
                        "Employee has been deleted successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    dgvEmployeeDetails.DataSource = employee.GetEmployees();
                    ClearControls();
                }
                else
                {
                    MessageBox.Show(
                        "Employee not found. Please try again.",
                        "Delete Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtEmpId.Text = "";
            txtEmpName.Text = "";
            txtAge.Text = "";
            txtContactNo.Text = "";
            cboGender.SelectedIndex = -1;
        }

        // Checks the required fields before adding or updating an employee.
        private bool ValidateInputs()
        {
            if (txtEmpId.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Employee ID.");
                txtEmpId.Focus();
                return false;
            }

            if (txtEmpName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Employee Name.");
                txtEmpName.Focus();
                return false;
            }

            int age;

            if (!int.TryParse(txtAge.Text.Trim(), out age))
            {
                MessageBox.Show("Please enter a valid age.");
                txtAge.Focus();
                return false;
            }

            if (age <= 0)
            {
                MessageBox.Show("Age must be greater than 0.");
                txtAge.Focus();
                return false;
            }

            if (cboGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a gender.");
                cboGender.Focus();
                return false;
            }

            return true;
        }

        private void dgvEmployeeDetails_RowHeaderMouseClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtEmpId.Text =
                dgvEmployeeDetails.Rows[e.RowIndex].Cells[0].Value?.ToString();

            txtEmpName.Text =
                dgvEmployeeDetails.Rows[e.RowIndex].Cells[1].Value?.ToString();

            txtAge.Text =
                dgvEmployeeDetails.Rows[e.RowIndex].Cells[2].Value?.ToString();

            txtContactNo.Text =
                dgvEmployeeDetails.Rows[e.RowIndex].Cells[3].Value?.ToString();

            cboGender.Text =
                dgvEmployeeDetails.Rows[e.RowIndex].Cells[4].Value?.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void cboGender_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dgvEmployeeDetails_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
        }

        private void txtEmpId_TextChanged(object sender, EventArgs e)
        {
        }
    }
}