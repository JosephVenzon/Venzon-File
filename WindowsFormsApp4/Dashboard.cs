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
            DataTable applications = Form1.ViewApplications(); // Call the static method to get the data
            dataGridView1.DataSource = applications; // Bind the data to the DataGridView
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
            string interviewDate = DatePicer.Value.ToString("yyyy-MM-dd HH:mm:ss");

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
            string interviewDate = STime.Value.ToString("yyyy-MM-dd HH:mm:ss");

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

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();

            MessageBox.Show("Successfully Log out!");
            form1.Show();
        }
    }
}
