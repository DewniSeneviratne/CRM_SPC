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
    public partial class stock : Form
    {
        private DatabaseHelper dbHelper;
        public stock()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            home home= new home();
            home.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LoadMedicineData()
        {
         
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();

                // Define the SQL query to retrieve data from the 'medicine' table
                string query = "SELECT * FROM medicine";

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        // Fill the DataTable with the results of the SQL query
                        dataAdapter.Fill(dataTable);

                        // Bind the DataGridView to the DataTable
                        dataGridView1.DataSource = dataTable;
                        DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
                        editButtonColumn.Name = "Edit";
                        editButtonColumn.Text = "Edit";
                        editButtonColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(editButtonColumn);

                        // Add the "Remove" button column
                        DataGridViewButtonColumn removeButtonColumn = new DataGridViewButtonColumn();
                        removeButtonColumn.Name = "Remove";
                        removeButtonColumn.Text = "Remove";
                        removeButtonColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(removeButtonColumn);
                    }
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label2.Show();
            label3.Show();
            label4.Show();
            label5.Show();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            textBox4.Show();
            button10.Show();
            dataGridView1.Hide();

            LoadMedicineData();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            label2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            textBox4.Hide();
            button10.Hide();
            dataGridView1.Show();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Get the values from the text boxes
            string name = textBox1.Text;
            if (!int.TryParse(textBox2.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            // Parse the expiration date (assuming it's entered as a string)
            if (!DateTime.TryParse(textBox3.Text, out DateTime exp_date))
            {
                MessageBox.Show("Please enter a valid expiration date.");
                return;
            }

            string description = textBox4.Text;

            // Create a DatabaseHelper instance
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Open a SQL connection
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                try
                {
                    connection.Open();

                    // Create an SQL command to insert data into the "medicine" table
                    string query = "INSERT INTO medicine (name, quantity, exp_date, desciption) " +
                        "VALUES (@name, @quantity, @exp_date, @description)";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@exp_date", exp_date);
                    command.Parameters.AddWithValue("@description", description);

                    // Execute the SQL command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully.");

                        // Clear the form after successful insertion
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void stock_Load(object sender, EventArgs e)
        {
            LoadMedicineData();
        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
                {
                    int medicineID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MID"].Value);

                    // Get other relevant data for the selected row
                    string medicineName = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    int quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value);
                    DateTime expDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["Exp_date"].Value);
                    string description = dataGridView1.Rows[e.RowIndex].Cells["Desciption"].Value.ToString();

                    EditMedicineForm editForm = new EditMedicineForm(medicineID, medicineName, quantity, expDate, description);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the DataGridView or perform any other actions needed
                        LoadMedicineData();
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "Remove")
                {
                    // Retrieve the medicine ID to be removed
                    int medicineID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MID"].Value);

                    // Ask for confirmation before removing
                    DialogResult result = MessageBox.Show("Are you sure you want to remove this medicine?", "Confirmation", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        // Call a method to remove the medicine record based on the medicineID
                        RemoveMedicine(medicineID);

                        // Refresh the DataGridView after removing
                        LoadMedicineData();
                    }
                }
            }
            }

            private void RemoveMedicine(int medicineID)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();

                using (SqlConnection connection = dbHelper.CreateConnection())
                {
                    connection.Open();

                    string query = "DELETE FROM medicine WHERE MID = @medicineID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@medicineID", medicineID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Medicine removed successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to remove medicine.");
                        }
                    }
                }
            } 

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
            stock i = new stock();
            i.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
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
            Prescriptions p = new Prescriptions();
            p.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
            staff p = new staff();
            p.Show();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
