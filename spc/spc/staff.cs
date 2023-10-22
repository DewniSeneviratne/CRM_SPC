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

namespace spc
{
    public partial class staff : Form
    {
        private DatabaseHelper dbHelper;
        public staff()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            stock i = new stock();
            i.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Delivery i = new Delivery();
            i.Show();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
            Delivery i = new Delivery();
            i.Show();
        }
     

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Prescriptions i = new Prescriptions();
            i.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            staff i = new staff();
            i.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 i = new Form1();
            i.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            home i = new home();
            i.Show();
        }

        private void staff_Load(object sender, EventArgs e)
        {
            LoadAuditLogData();
        }
        private void LoadAuditLogData()
        {
            using (var connection = dbHelper.CreateConnection())
            {
                connection.Open();

                string query = "SELECT LogID, TableName, Operation, RecordID, Timestamp FROM AuditLog";

                using (var dataAdapter = new SqlDataAdapter(query, connection))
                {
                    using (var dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }
    }
}
