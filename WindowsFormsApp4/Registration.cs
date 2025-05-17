using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Registration: Form
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }
        private void Registration_Load(object sender, EventArgs e)
        {

        }


        private void Register_Click(object sender, EventArgs e)
        {
            string username = user.Text.Trim();
            string password = pass.Text;
            string email = gmail.Text.Trim();

            // Validate input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Complete the form first.");
                return;
            }

            // Require @gmail.com email
            if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Email must be a valid @gmail.com address.");
                return;
            }

            // Proceed to register
            Form1.RegisterUser(username, password, email);

            MessageBox.Show("Registration successful!");

            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
