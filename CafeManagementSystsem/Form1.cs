using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystsem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Documents\Cafedb.mdf;Integrated Security=True;Connect Timeout=30");
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Guest login
            this.Hide();
            GuestOrder guest = new GuestOrder();
            guest.Show();
        }

        public static string user;
        private void button1_Click(object sender, EventArgs e)
        {
            // User login
            if(uname.Text == "")
            {
                MessageBox.Show("Please enter your username");
            }
            else if(upass.Text == "")
            {
                MessageBox.Show("Please enter your password");
            }
            else
            {
                try
                {
                    Con.Open();

                    // Check if username exists
                    string checkUserQuery = "SELECT Upassword FROM UserTbl WHERE Uname = @uname";
                    SqlCommand cmd = new SqlCommand(checkUserQuery, Con);
                    cmd.Parameters.AddWithValue("@uname", uname.Text);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        // Username not found
                        MessageBox.Show("User does not exist.");
                        uname.Clear();
                        upass.Clear();
                        uname.Focus();
                    }
                    else
                    {
                        string correctPassword = result.ToString().Trim();

                        if (upass.Text == correctPassword)
                        {
                            // Login successful
                            user = uname.Text;

                            UserOrder uorder = new UserOrder();
                            uorder.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Password incorrect
                            MessageBox.Show("Incorrect password.");
                            upass.Clear();
                            upass.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }
    }
}
