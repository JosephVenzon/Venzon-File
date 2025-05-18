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
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using Google.Protobuf.WellKnownTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp4
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        static string connectionString = "Server=localhost;Database=JobData;User ID=root;Password=;";
        static User loggedInUser = null;

        private void Login_Click(object sender, EventArgs e)
        {
            string enteredUsername = Username.Text.Trim();
            string enteredPassword = Password.Text;

            LoginUser(enteredUsername, enteredPassword, this);
        }

        static void LoginUser(string username, string password, Form1 form1)
        {
            Registration regs = new Registration();
            User user = GetUserFromDatabase(username);

            if (user != null)
            {

                if (user.Password == password)
                {
                    Dashboard dash = new Dashboard();
                    dash.Show();
                    form1.Hide();
                    loggedInUser = user;

                    SetEmail("Log In Notification",username, $"Hello {username} you have been log in successfully", user.Gmail);
                    

                }
                else
                {
                    form1.Alert.Text = "Incorrect Password";
                }
            }
            else
            {
                form1.Alert.Text = "User not found. Register first";
            }
        }

        static void SetEmail(string type, string name, string text, string emailAdd)
        {
            // Validate email address
            if (string.IsNullOrWhiteSpace(emailAdd))
            {
                Console.WriteLine("Recipient email address is missing.");
                return;
            }

            try
            {
                MailAddress to = new MailAddress(emailAdd);
                MailAddress from = new MailAddress("davevenzon789@gmail.com");

                MailMessage email = new MailMessage(from, to)
                {
                    Subject = type,
                    Body = text
                };

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("davevenzon789@gmail.com", "jxmhfuvaeckvrtfu"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true
                };

                smtp.Send(email);
                Console.WriteLine("Email sent successfully to " + emailAdd);
            }
            catch (FormatException)
            {
                Console.WriteLine("The provided email address is not in a valid format: " + emailAdd);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }


        private void Signup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration regs = new Registration();
            this.Hide();
            regs.Show();
        }


        public static bool RegisterUser(string username, string password, string gmail)
        {
            if (UserExists(username))
            {
                MessageBox.Show("Username already exists");
                return false;
            }

            string role = username.EndsWith("myadmin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";

            AddUserToDatabase(username, password, gmail, role);
            return true;
        }





        public static bool UserExists(string username)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        static void AddUserToDatabase(string username, string password, string gmail, string role)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Id, Username, Password, Gmail, Role) VALUES (@Id, @Username, @Password, @Gmail, @Role)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Gmail", gmail);
                cmd.Parameters.AddWithValue("@Role", role);

                cmd.ExecuteNonQuery();
            }
        }


        static User GetUserFromDatabase(string username)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Username, Password, Gmail, Role FROM Users WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User(
                            reader["Id"].ToString(),
                            reader["Username"].ToString(),
                            reader["Password"].ToString(),
                            reader["Gmail"].ToString(),
                            reader["Role"].ToString()
                        );
                    }
                    return null;
                }
            }
        }



        public static void AddJobApplicationToDatabase(string company, string jobTitle, string resumePath, string coverLetterPath)
        {
            string connectionString = "Server=localhost;Database=JobData;User ID=root;Password=;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO JobApplications (Id, CompanyName, JobTitle, ResumePath, CoverLetterPath, Status, UserId) " +
                               "VALUES (@Id, @CompanyName, @JobTitle, @ResumePath, @CoverLetterPath, 'Applied', @UserId)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@CompanyName", company);
                cmd.Parameters.AddWithValue("@JobTitle", jobTitle);
                cmd.Parameters.AddWithValue("@ResumePath", resumePath);
                cmd.Parameters.AddWithValue("@CoverLetterPath", coverLetterPath);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);
                cmd.ExecuteNonQuery();
            }
        }


        public static DataTable ViewApplications()
        {
            DataTable dt = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CompanyName, JobTitle, Status, Id, " +
                               "IFNULL(DATE_FORMAT(InterviewDate, '%Y-%m-%d'), '') AS InterviewDate " +
                               "FROM JobApplications WHERE UserId = @UserId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable ViewDataList()
        {
            DataTable dt = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query;

                if (loggedInUser.Role == "Admin")
                {
                    query = "SELECT CompanyName, JobTitle, Status, Id, " +
                            "IFNULL(DATE_FORMAT(InterviewDate, '%Y-%m-%d'), '') AS InterviewDate " +
                            "FROM JobApplications";
                }
                else
                {
                    query = "SELECT CompanyName, JobTitle, Status, Id, " +
                            "IFNULL(DATE_FORMAT(InterviewDate, '%Y-%m-%d'), '') AS InterviewDate " +
                            "FROM JobApplications WHERE UserId = @UserId";
                }

                MySqlCommand cmd = new MySqlCommand(query, connection);

                if (loggedInUser.Role != "Admin")
                {
                    cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);
                }

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }


        public static DataTable ViewUserInformation()
        {
            DataTable dt = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Username, Id, Role, Gmail, Password FROM Users WHERE Id = @UserId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }





        public static bool Value = true;
        public static void UpdateApplicationStatus(string status, string appId)
        {
            if (loggedInUser.Role != "Admin")
            {
                MessageBox.Show("Only admins can update application status.");
                Value = false;
                return;
            }

            string[] allowedStatuses = { "Interviewed", "Hired", "Rejected" };
            if (!allowedStatuses.Contains(status))
            {
                MessageBox.Show("Allowed values are: Interviewed, Hired, Rejected.");
                Value = false;
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE JobApplications SET Status = @Status WHERE Id = @AppId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@AppId", appId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    MessageBox.Show("Invalid ID. Input correctly");
                    Value = false;
                }
                else
                {
                    Value = true;
                    Console.WriteLine("Application status updated successfully.");
                }
            }
        }


        public static bool ScheduleInterview(string appId, string interviewDate)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE JobApplications SET InterviewDate = @InterviewDate WHERE Id = @AppId AND UserId = @UserId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@InterviewDate", interviewDate);
                cmd.Parameters.AddWithValue("@AppId", appId);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public static bool DeleteJobApplication(string appId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM JobApplications WHERE Id = @AppId AND UserId = @UserId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@AppId", appId);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }



        public static bool SetFollowUpReminder(string appId, string interviewDate)
        {


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE JobApplications SET FollowUpDate = @FollowUpDate WHERE Id = @AppId AND UserId = @UserId";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FollowUpDate", interviewDate);
                cmd.Parameters.AddWithValue("@AppId", appId);
                cmd.Parameters.AddWithValue("@UserId", loggedInUser.Id);
                cmd.ExecuteNonQuery();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }


        }

        private void Password_TextChanged(object sender, EventArgs e)
        {

        }
    }



    class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gmail { get; set; }
        public string Role { get; set; }
        public User(string username, string password, string gmail)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            Gmail = gmail;
            Role = "User";
        }
        public User(string id, string username, string password, string gmail, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Gmail = gmail;
            Role = role;
        }
    }



    class JobApplication
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string ResumePath { get; set; }
        public string CoverLetterPath { get; set; }
        public string Status { get; set; } = "Applied";
        public DateTime? InterviewDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}
