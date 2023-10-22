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
    public partial class Delivery : Form
    {
        public Delivery()
        {
            InitializeComponent();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
            stock s = new stock();
            s.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Delivery d = new Delivery();
            d.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Customers c = new Customers();
            c.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Prescriptions i = new Prescriptions();
            i.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
            staff i = new staff();
            i.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Show();
            comboBox1.Show();
            textBox3.Show();
            comboBox2.Show();
            comboBox3.Show();
            textBox6.Show();
            label2.Show();
            label3.Show();
            label4.Show();
            label5.Show();
            label6.Show();
            label7.Show();
            dataGridView1.Hide();
            button10.Show();
        }
        private void button10_Click(object sender, EventArgs e)
        {

            string d_date = textBox1.Text;
            string package_status = comboBox1.SelectedItem.ToString();
            string notes = textBox3.Text;
            string delivery_status = comboBox2.SelectedItem.ToString();
            string payment_status = comboBox3.SelectedItem.ToString();
            int PID = int.Parse(textBox6.Text); 

            DatabaseHelper dbHelper = new DatabaseHelper();
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();

                string query = "INSERT INTO deliver (d_date, package_status, notes, delivery_status, payement_status, PID) " +
                               "VALUES (@d_date, @package_status, @notes, @delivery_status, @payment_status, @PID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@d_date", d_date);
                    command.Parameters.AddWithValue("@package_status", package_status);
                    command.Parameters.AddWithValue("@notes", notes);
                    command.Parameters.AddWithValue("@delivery_status", delivery_status);
                    command.Parameters.AddWithValue("@payment_status", payment_status);
                    command.Parameters.AddWithValue("@PID", PID);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Delivery details added successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add delivery details.");
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Hide();
            comboBox1.Hide();
            textBox3.Hide();
            comboBox2.Hide();
            comboBox3.Hide();
            textBox6.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            label6.Hide();
            label7.Hide();
            dataGridView1.Show();
            button10.Hide();


            // Use DatabaseHelper to create a connection
            DatabaseHelper dbHelper = new DatabaseHelper();
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();

                // Define the SQL query to retrieve data from the 'delivery' table
                string query = "SELECT * FROM deliver";

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        // Fill the DataTable with the results of the SQL query
                        dataAdapter.Fill(dataTable);

                        // Bind the DataGridView to the DataTable
                        dataGridView1.DataSource = dataTable;

                        // Add an "Edit" button column
                        DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
                        editButtonColumn.HeaderText = "Edit";
                        editButtonColumn.Text = "Edit";
                        editButtonColumn.Name = "Edit";
                        editButtonColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(editButtonColumn);

                        
                    }
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                int selectedDID = (int)dataGridView1.Rows[e.RowIndex].Cells["DID"].Value;

               
                EditDeliveryForm editForm = new EditDeliveryForm(selectedDID);
                editForm.ShowDialog();
            }
        }

        private void Delivery_Load(object sender, EventArgs e)
        {
            textBox1.Show();
            comboBox1.Show();
            textBox3.Show();
            comboBox2.Show();
            comboBox3.Show();
            textBox6.Show();
            label2.Show();
            label3.Show();
            label4.Show();
            label5.Show();
            label6.Show();
            label7.Show();
            dataGridView1.Hide();
            button10.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }
    }
}

