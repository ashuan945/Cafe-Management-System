using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CafeManagementSystsem
{
    public partial class UserOrder : Form
    {
        public UserOrder()
        {
            InitializeComponent();
        }

        // Database connection
        SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Documents\Cafedb.mdf;Integrated Security=True;Connect Timeout=30");

        // Variables for order
        int orderNumber = 0;
        decimal price, total;
        string itemName, itemCategory;
        int sum = 0;

        DataTable orderTable = new DataTable();

        private void UserOrder_Load(object sender, EventArgs e)
        {
            // Populate Items DataGridView
            populate();

            // Initialize order DataTable
            orderTable.Columns.Add("Order Number", typeof(int));
            orderTable.Columns.Add("Item Name", typeof(string));
            orderTable.Columns.Add("Category", typeof(string));
            orderTable.Columns.Add("Unit Price", typeof(decimal));
            orderTable.Columns.Add("Total", typeof(decimal));

            CartGV.DataSource = orderTable;

            // Default quantity
            Qty.Text = "1";

            // Date
            Datelbl.Text = DateTime.Today.Day.ToString() + "/"
                + DateTime.Today.Month.ToString() + "/"
                + DateTime.Today.Year.ToString();

            // Load username
            Seller.Text = Form1.user;

            // Set initial amount
            LabelAmnt.Text = "0";

            // Set the order number automatically
            SetNextOrderNumber();
        }

        // Get Next Order Number
        private void SetNextOrderNumber()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(OrderNum), 0) FROM OrderTbl", Con);
                int lastOrderNum = Convert.ToInt32(cmd.ExecuteScalar());
                OrderNum.Text = (lastOrderNum + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching last order number: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Populate
        void populate()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM ItemTbl";
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                ItemsGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching items: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Filter by Category
        void filterbycategory()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM ItemTbl WHERE ItemCat = '" +
                     cat.SelectedItem.ToString() + "'";
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                ItemsGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching items: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Navigate to UsersForm
        private void button4_Click(object sender, EventArgs e)
        {
            UsersForm uform = new UsersForm();
            uform.Show();
            this.Hide();
        }

        // Logout / go back to login
        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        // Navigate to ItemsForm
        private void button3_Click(object sender, EventArgs e)
        {
            ItemsForm itemForm = new ItemsForm();
            itemForm.Show();
            this.Hide();
        }

        // Exit application
        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Select item from ItemsGV
        private void ItemsGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                itemName = ItemsGV.Rows[e.RowIndex].Cells["ItemName"].Value.ToString();
                itemCategory = ItemsGV.Rows[e.RowIndex].Cells["ItemCat"].Value.ToString();
                price = Convert.ToDecimal(ItemsGV.Rows[e.RowIndex].Cells["ItemPrice"].Value);
            }
        }

        private void ItemsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            filterbycategory();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Reset category selection
            cat.SelectedIndex = -1;    // Deselect any item
            cat.Text = "Category";     // Show placeholder text

            // Refresh items
            populate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if cart is empty
            if (orderTable.Rows.Count == 0)
            {
                MessageBox.Show("Cannot place order. The cart is empty.");
                return; // exit the method
            }

            try
            {
                Con.Open();

                // Add to order
                string query = "INSERT INTO OrderTbl (OrderNum, OrderDate, [User], OrderAmount) VALUES (@onum, @odate, @ouser, @oamt)";
                SqlCommand cmd = new SqlCommand(query, Con);

                cmd.Parameters.AddWithValue("@onum", OrderNum.Text);
                cmd.Parameters.AddWithValue("@odate", Datelbl.Text);
                cmd.Parameters.AddWithValue("@ouser", Seller.Text);
                cmd.Parameters.AddWithValue("@oamt", LabelAmnt.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Order Successfully Created");
                Con.Close();

                // Clear the cart
                orderTable.Clear();
                CartGV.DataSource = orderTable;

                // Reset total
                sum = 0;
                LabelAmnt.Text = "0";

                // Reset next order number
                SetNextOrderNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ViewOrders view = new ViewOrders();
            view.Show();
        }

        private void OrderNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelamt_Click(object sender, EventArgs e)
        {

        }

        // Add selected item to order
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Qty.Text))
            {
                MessageBox.Show("Enter the quantity of the item.");
                return;
            }

            if (string.IsNullOrEmpty(itemName))
            {
                MessageBox.Show("Select a product to be ordered.");
                return;
            }

            if (!int.TryParse(Qty.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Enter a valid quantity greater than 0.");
                return;
            }

            orderNumber++;
            total = price * quantity;
            orderTable.Rows.Add(orderNumber, itemName, itemCategory, price, total);
            CartGV.DataSource = orderTable;
            sum = (int)(sum + total);
            LabelAmnt.Text = sum.ToString();

            // Reset selected item and quantity
            itemName = "";
            Qty.Text = "1";
        }

        private void OrderGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // You can leave this empty for now
        }

        
    }
}