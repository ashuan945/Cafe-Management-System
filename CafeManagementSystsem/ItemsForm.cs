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
    public partial class ItemsForm : Form
    {
        public ItemsForm()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Documents\Cafedb.mdf;Integrated Security=True;Connect Timeout=30");

        void populate()
        {
            Con.Open();
            string query = "SELECT * FROM ItemTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemsGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        void clearInput()
        {
            // Clear Input
            ItemNum.Clear();
            ItemName.Clear();
            ItemCat.SelectedIndex = -1;
            ItemCat.Text = "";
            Price.Clear();
            generateItemNum();
        }

        // Generate the Next ItemNum
        void generateItemNum()
        {
            try
            {
                Con.Open();
                string query = "SELECT MAX(CAST(ItemNum AS INT)) FROM ItemTbl";
                SqlCommand cmd = new SqlCommand(query, Con);
                object result = cmd.ExecuteScalar();

                int nextNum = 1; // default if table empty

                if (result != DBNull.Value)
                {
                    nextNum = Convert.ToInt32(result) + 1;
                }

                ItemNum.Text = nextNum.ToString();
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

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UsersForm uform = new UsersForm();
            uform.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserOrder uorder = new UserOrder();
            uorder.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ItemsGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ItemNum.Text = ItemsGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                ItemName.Text = ItemsGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                ItemCat.SelectedItem = ItemsGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                Price.Text = ItemsGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void ItemsForm_Load(object sender, EventArgs e)
        {
            populate();
            generateItemNum();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ItemNum.Text == "" || ItemName.Text == "" || Price.Text == "" || ItemCat.SelectedIndex == -1)
            {
                MessageBox.Show("Fill in All the Data");
                return;
            }

            // Parse price as decimal
            if (!decimal.TryParse(Price.Text, out decimal itemPrice))
            {
                MessageBox.Show("Invalid price. Please enter a number.");
                Price.Focus();
                return;
            }
            // Round to 2 decimals
            itemPrice = Math.Round(itemPrice, 2);
            Price.Text = itemPrice.ToString("F2"); // show 2 decimals

            try
            {
                Con.Open();

                // Check if ItemName already exists
                string checkNameQuery = "SELECT COUNT(*) FROM ItemTbl WHERE ItemName = @iname";
                SqlCommand checkCmd = new SqlCommand(checkNameQuery, Con);
                checkCmd.Parameters.AddWithValue("@iname", ItemName.Text.Trim());
                int nameCount = (int)checkCmd.ExecuteScalar();
                if (nameCount > 0)
                {
                    MessageBox.Show("Item name already exists. Please use a different name.");
                    return;
                }

                // Insert new item
                string query = "INSERT INTO ItemTbl (ItemNum, ItemName, ItemCat, ItemPrice) VALUES (@inum, @iname, @icat, @iprice)";
                SqlCommand cmd = new SqlCommand(query, Con);

                cmd.Parameters.AddWithValue("@inum", ItemNum.Text);
                cmd.Parameters.AddWithValue("@iname", ItemName.Text);
                cmd.Parameters.AddWithValue("@icat", ItemCat.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@iprice", Price.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Item Successfully Created");

                // Clear Input
                ItemNum.Clear();
                ItemName.Clear();
                ItemCat.SelectedIndex = -1;
                ItemCat.Text = "";
                Price.Clear();
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

        private void button2_Click(object sender, EventArgs e)
        {

            if (ItemNum.Text == "" || ItemName.Text == "" || ItemCat.SelectedIndex == -1 || Price.Text == "")
            {
                MessageBox.Show("Fill in all the Fields.");
                return;
            }

            // Parse price as decimal
            if (!decimal.TryParse(Price.Text, out decimal itemPrice))
            {
                MessageBox.Show("Invalid price. Please enter a number.");
                Price.Focus();
                return;
            }

            // Round to 2 decimals
            itemPrice = Math.Round(itemPrice, 2);
            Price.Text = itemPrice.ToString("F2"); // show 2 decimals


            try
                {
                    Con.Open();

                // Check if ItemName already exists in another record
                string checkQuery = "SELECT COUNT(*) FROM ItemTbl WHERE ItemName = @iname AND ItemNum <> @inum";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Con);
                checkCmd.Parameters.AddWithValue("@iname", ItemName.Text.Trim());
                checkCmd.Parameters.AddWithValue("@inum", ItemNum.Text.Trim());
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Another item with this name already exists. Please choose a different name.");
                    return;
                }

                // Update item
                string updateQuery = "UPDATE ItemTbl SET ItemName = @iname, ItemCat = @icat, ItemPrice = @iprice WHERE ItemNum = @inum";
                SqlCommand cmd = new SqlCommand(updateQuery, Con);
                cmd.Parameters.AddWithValue("@inum", ItemNum.Text.Trim());
                cmd.Parameters.AddWithValue("@iname", ItemName.Text.Trim());
                cmd.Parameters.AddWithValue("@icat", ItemCat.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@iprice", itemPrice);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item succesfully updated!");
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ItemNum.Text == "")
            {
                MessageBox.Show("Select the Item to be Deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "DELETE FROM ItemTbl WHERE ItemNum = '"
                        + ItemNum.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item succesfully deleted!");
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

        private void ItemsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            clearInput();
        }
    }
}
