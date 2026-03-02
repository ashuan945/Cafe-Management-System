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
    public partial class ViewOrders : Form
    {
        public ViewOrders()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Documents\Cafedb.mdf;Integrated Security=True;Connect Timeout=30");

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void populate()
        {
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [OrderTbl]", Con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    OrdersGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching orders: " + ex.Message);
            }
        }

        private void ViewOrders_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void OrdersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (OrdersGV.CurrentRow == null)
            {
                MessageBox.Show("No order selected.");
                return;
            }

            string orderNum = OrdersGV.CurrentRow.Cells[0].Value.ToString();
            string orderDate = OrdersGV.CurrentRow.Cells[1].Value.ToString();
            string User = OrdersGV.CurrentRow.Cells[2].Value.ToString();
            string orderAmount = OrdersGV.CurrentRow.Cells[3].Value.ToString();

            e.Graphics.DrawString("======= Ashuan's Cafe =======",
                new Font("Century", 24, FontStyle.Bold),
                Brushes.Red, new Point(140, 30));

            e.Graphics.DrawString("====== Order Summary ======",
                new Font("Century", 18, FontStyle.Bold),
                Brushes.Red, new Point(220, 90));

            e.Graphics.DrawString(
                "Order Number: " + orderNum,
                new Font("Century", 14),
                Brushes.Black, new Point(100, 160));

            e.Graphics.DrawString(
                "Order Date: " + orderDate,
                new Font("Century", 14),
                Brushes.Black, new Point(100, 190));

            e.Graphics.DrawString(
                "Seller: " + User,
                new Font("Century", 14),
                Brushes.Black, new Point(100, 220));

            e.Graphics.DrawString(
                "Total Amount: RM " + orderAmount,
                new Font("Century", 14),
                Brushes.Black, new Point(100, 250));

            e.Graphics.DrawString("====== End of Order Summary ======",
                new Font("Century", 18, FontStyle.Bold),
                Brushes.Red, new Point(170, 320));
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (OrdersGV.CurrentRow == null)
            {
                MessageBox.Show("Please select an order first.");
                return;
            }

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
    }
}
