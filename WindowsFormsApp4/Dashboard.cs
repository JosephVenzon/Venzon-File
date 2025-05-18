using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp4
{
    public partial class Dashboard: Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Info.CellClick += Info_CellClick;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {

        }

        private void tabAddApplication_Click(object sender, EventArgs e)
        {

        }

        static string connectionString = "Server=localhost;Database=jobdatabase;User ID=root;Password=;";
        static User loggedInUser = null;

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            
        }

        public static void RegisterUser(string username, string password, string gmail)
        {
            
        }
        private void Register_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Campany.Text) || string.IsNullOrEmpty(Job.Text) || string.IsNullOrEmpty(Resume.Text) || string.IsNullOrEmpty(Cover.Text))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            Form1.AddJobApplicationToDatabase(Campany.Text, Job.Text, Resume.Text, Cover.Text);

            MessageBox.Show("Application added successfully!");
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            
        }
        private void guna2GradientButton1_Click_2(object sender, EventArgs e)
        {
            DataTable applications = Form1.ViewApplications();
            dataGridView1.DataSource = applications;
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void tabViewApplications_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ID.Text) || string.IsNullOrEmpty(STATUS.Text))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            Form1.UpdateApplicationStatus(STATUS.Text, ID.Text);

            if (Form1.Value == true)
            {
                MessageBox.Show("Status updated successfully!");
            }
        }


        private void guna2GradientButton3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SID.Text))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            string appId = SID.Text;
            string interviewDate = DatePicer.Value.ToString("yyyy-MM-dd");

            bool success = Form1.ScheduleInterview(appId, interviewDate);

            if (success)
            {
                MessageBox.Show("Interview scheduled successfully.");
            }
            else
            {
                MessageBox.Show("Invalid Application ID. Please check and try again.");
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TID.Text))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            string appId = TID.Text;
            string interviewDate = STime.Value.ToString("yyyy-MM-dd");

            bool success = Form1.SetFollowUpReminder(appId, interviewDate);

            if (success)
            {
                MessageBox.Show("Follo-Up scheduled successfully.");
            }
            else
            {
                MessageBox.Show("Invalid Application ID");
            }
        }

        

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void Job_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }

        private void Resume_TextChanged(object sender, EventArgs e)
        {

        }

        private void DatePicer_ValueChanged(object sender, EventArgs e)
        {

        }

        private void STime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();

            MessageBox.Show("Successfully Log out!");
            form1.Show();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            DataTable applications = Form1.ViewApplications();
            DeleteBoard.DataSource = applications;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string appIdToDelete = Deletebox.Text.Trim();

            if (string.IsNullOrEmpty(appIdToDelete))
            {
                MessageBox.Show("Please enter a valid Application ID.");
                return;
            }

            if (string.IsNullOrEmpty(Deletebox.Text))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            bool deleted = Form1.DeleteJobApplication(appIdToDelete);

            if (deleted)
            {
                MessageBox.Show("Job application deleted successfully.");
            }
            else
            {
                MessageBox.Show("Invalid ID. Deletion failed.");
            }

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            DataTable applications = Form1.ViewApplications();
            SView.DataSource = applications;
        }

        private void guna2GradientButton8_Click(object sender, EventArgs e)
        {
            FView.AllowUserToAddRows = false;
            DataTable applications = Form1.ViewApplications();
            FView.DataSource = applications;
        }

        private void guna2HtmlLabel15_Click(object sender, EventArgs e)
        {

        }

        private void STATUS_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton9_Click(object sender, EventArgs e)
        {
            DataTable applications = Form1.ViewApplications();
            UView.DataSource = applications;
        }

        private void guna2GradientButton10_Click(object sender, EventArgs e)
        {
            Info.AllowUserToAddRows = false;

            DataTable userInfo = Form1.ViewUserInformation();
            Info.DataSource = userInfo;

            
            if (!Info.Columns.Contains("TogglePassword"))
            {
                var toggleColumn = new DataGridViewButtonColumn
                {
                    Name = "TogglePassword",
                    HeaderText = "Password",
                    UseColumnTextForButtonValue = false
                };
                Info.Columns.Add(toggleColumn);
            }

            foreach (DataGridViewRow row in Info.Rows)
            {
                if (row.Cells["Password"].Value != null)
                {
                    string realPassword = row.Cells["Password"].Value.ToString();
                    row.Cells["Password"].Tag = realPassword;
                    row.Cells["Password"].Value = new string('*', realPassword.Length);
                    row.Cells["TogglePassword"].Value = "Show";
                }
            }
        }


        private void Info_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && Info.Columns[e.ColumnIndex].Name == "TogglePassword")
            {
                DataGridViewRow row = Info.Rows[e.RowIndex];
                DataGridViewCell passwordCell = row.Cells["Password"];
                string actualPassword = passwordCell.Tag?.ToString();
                if (actualPassword == null) return;

                bool isMasked = passwordCell.Value.ToString().StartsWith("*");
                passwordCell.Value = isMasked ? actualPassword : new string('*', actualPassword.Length);
                row.Cells["TogglePassword"].Value = isMasked ? "Hide" : "Show";
            }
        }
    }


}
