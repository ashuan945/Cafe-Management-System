using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystsem
{
    public partial class UsersForm : Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Documents\Cafedb.mdf;Integrated Security=True;Connect Timeout=30");
        
        void populate()
        {
            Con.Open();
            string query = "SELECT * FROM UserTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UsersGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        void clearInput()
        {
            // Clear Input
            unameTb.Clear();
            UphoneTb.Clear();
            UpassTb.Clear();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserOrder uorder = new UserOrder();
            uorder.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ItemsForm Item = new ItemsForm();
            Item.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (unameTb.Text == "" || UphoneTb.Text == "" || UpassTb.Text == "")
            {
                MessageBox.Show("Missing Information");
                return;
            }

            try
            {
                Con.Open();

                // Check if username already exists
                string checkQuery = "SELECT COUNT(*) FROM UserTbl WHERE Uname = @name";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Con);
                checkCmd.Parameters.AddWithValue("@name", unameTb.Text);
                int userCount = (int)checkCmd.ExecuteScalar();

                if (userCount > 0)
                {
                    MessageBox.Show("Username already exists. Please choose a different username.");
                    return; // stop further execution
                }

                // Validate phone number
                string phonePattern = @"^(?:\+60|0)1[0-9]-?\d{7,8}$";
                if (!Regex.IsMatch(UphoneTb.Text, phonePattern))
                {
                    MessageBox.Show("Invalid Malaysian phone number. Example: 0123456789 or +60123456789");
                    return;
                }

                // Check if phone number already exists
                string checkPhone = "SELECT COUNT(*) FROM UserTbl WHERE Uphone = @phone";
                SqlCommand checkPhoneCmd = new SqlCommand(checkPhone, Con);
                checkPhoneCmd.Parameters.AddWithValue("@phone", UphoneTb.Text);
                int phoneCount = (int)checkPhoneCmd.ExecuteScalar();
                if (phoneCount > 0)
                {
                    MessageBox.Show("Phone number already exists. Please use a different number.");
                    return;
                }


                // Password Validation
                if (UpassTb.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.");
                    return;
                }


                // Insert new user
                string query = "INSERT INTO UserTbl (Uname, Uphone, Upassword) VALUES (@name, @phone, @pass)";
                SqlCommand cmd = new SqlCommand(query, Con);

                cmd.Parameters.AddWithValue("@name", unameTb.Text);
                cmd.Parameters.AddWithValue("@phone", UphoneTb.Text);
                cmd.Parameters.AddWithValue("@pass", UpassTb.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User Successfully Created");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Con.Close();
                clearInput();
                populate();
            }
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void UsersGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                unameTb.Text = UsersGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                UphoneTb.Text = UsersGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                UpassTb.Text = UsersGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (UphoneTb.Text == "" || UpassTb.Text == "" || unameTb.Text == "")
            {
                MessageBox.Show("Fill in all the Fields.");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "UPDATE UserTbl SET Uname = '" + unameTb.Text +
                        "', Upassword = '" + UpassTb.Text + 
                        "' WHERE Uphone = '" + UphoneTb.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User succesfully updated!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Con.Close();
                    clearInput();
                    populate();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(UphoneTb.Text == "")
            {
                MessageBox.Show("Select the User to be Deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "DELETE FROM UserTbl WHERE Uphone = '"
                        + UphoneTb.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User succesfully deleted!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Con.Close();
                    clearInput();
                    populate();
                }
            }
        }

        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void unameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            clearInput();
        }
    }
}
