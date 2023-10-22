using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spc
{
    public partial class Customers : Form
    {
        private DatabaseHelper dbHelper;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
        private string selectedCID;
        public Customers()
        {
            InitializeComponent();
            InitializeDataGridView();
           
            dbHelper = new DatabaseHelper();
            LoadData();

        }

        private void InitializeDataGridView()
        {
            // Add columns to the DataGridView for the columns you want to display
  

            // Set the column visibility as needed, e.g., hide CID
            
        }

        private void LoadData()
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM customer", connection))
                {
                    using (DataSet dataSet = new DataSet())
                    {
                        dataAdapter.Fill(dataSet, "customer");

                        using (SqlDataAdapter loyalCustomerDataAdapter = new SqlDataAdapter("SELECT * FROM loyal_customer", connection))
                        {
                            loyalCustomerDataAdapter.Fill(dataSet, "loyal_customer");

                            // combine data from both tables
                            dataSet.Relations.Add("Customer_LoyalCustomer", dataSet.Tables["customer"].Columns["CID"], dataSet.Tables["loyal_customer"].Columns["CID"]);

                            foreach (DataRow customerRow in dataSet.Tables["customer"].Rows)
                            {
                                DataRow[] relatedLoyalCustomers = customerRow.GetChildRows("Customer_LoyalCustomer");

                                if (relatedLoyalCustomers.Length > 0)
                                {
                            
                                    string address = relatedLoyalCustomers[0]["address"].ToString();
                                    string email = relatedLoyalCustomers[0]["email"].ToString();
                                    
                                    dataGridView1.Rows.Add(customerRow["CID"], customerRow["name"], customerRow["tel_num"], address, email);
                                }
                                else
                                {
                               
                                    dataGridView1.Rows.Add(customerRow["CID"], customerRow["name"], customerRow["tel_num"], "", "");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Customers_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            string name = textBox1.Text;
            string telNum = textBox2.Text;

         
            DatabaseHelper dbHelper = new DatabaseHelper();

        
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Customer (name, tel_num) VALUES (@name, @telNum)";
                    SqlCommand command = new SqlCommand(query, connection);

                   
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@telNum", telNum);

           
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data inserted successfully.");
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

        private void button8_Click(object sender, EventArgs e)
        {
            label1.Hide();
            label2.Hide();
            textBox1.Hide();
            textBox2.Hide();
            button10.Hide();
            dataGridView1.Rows.Clear();
            dataGridView1.Show();
            LoadData();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label1.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            button10.Show();
            dataGridView1.Hide();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                e.Value = "Edit";
            }
            if (e.ColumnIndex == 6)
            {
                e.Value = "Remove";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "edit")
                {
                    selectedCID = dataGridView1.Rows[e.RowIndex].Cells["CID"].Value.ToString(); // Store the CID for editing

                    
                    EditCustomerForm editForm = new EditCustomerForm();

                    editForm.CID = selectedCID;
                    editForm.Name = dataGridView1.Rows[e.RowIndex].Cells["name"].Value.ToString();
                    editForm.TelNum = dataGridView1.Rows[e.RowIndex].Cells["tel_num"].Value.ToString();
                    editForm.Address = dataGridView1.Rows[e.RowIndex].Cells["address"].Value.ToString();
                    editForm.Email = dataGridView1.Rows[e.RowIndex].Cells["email"].Value.ToString();

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        string updatedName = editForm.Name;
                        string updatedTelNum = editForm.TelNum;
                        string updatedAddress = editForm.Address;
                        string updatedEmail = editForm.Email;

                        // Update
                        using (SqlConnection connection = dbHelper.CreateConnection())
                        {
                            connection.Open();
                            string query = "UPDATE customer SET name = @name, tel_num = @telNum WHERE CID = @cid";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@name", updatedName);
                            command.Parameters.AddWithValue("@telNum", updatedTelNum);
                            command.Parameters.AddWithValue("@cid", selectedCID); 

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                if (!string.IsNullOrEmpty(updatedAddress) && !string.IsNullOrEmpty(updatedEmail))
                                {
                                
                                    query = "SELECT COUNT(*) FROM loyal_customer WHERE CID = @cid";
                                    command = new SqlCommand(query, connection);
                                    command.Parameters.AddWithValue("@cid", selectedCID);
                                    int count = (int)command.ExecuteScalar();

                            
                                    if (count > 0)
                                    {
                                        query = "UPDATE loyal_customer SET address = @address, email = @email WHERE CID = @cid";
                                    }
                                    else
                                    {
                                        // If no record exists, insert a new one
                                        query = "INSERT INTO loyal_customer (CID, address, email) VALUES (@cid, @address, @email)";
                                    }

                                    command = new SqlCommand(query, connection);
                                    command.Parameters.AddWithValue("@cid", selectedCID);
                                    command.Parameters.AddWithValue("@address", updatedAddress);
                                    command.Parameters.AddWithValue("@email", updatedEmail);

                                    rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Data updated successfully.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update data.");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Failed to update data.");
                            }
                        }
                    }
                }
            }
            }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            stock i = new stock();
            i.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            Customers c = new Customers();
            c.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Prescriptions p = new Prescriptions();
            p.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            staff p = new staff();
            p.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Delivery d = new Delivery();
            d.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
