using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spc
{
    public partial class EditDeliveryForm : Form
    {
        private int DID;
        public EditDeliveryForm(int did)
        {
            InitializeComponent();
            DID = did;
            textBox3.Text = DID.ToString();
            LoadDeliveryDetails();
        }

        private void LoadDeliveryDetails()
        {
            
            DatabaseHelper dbHelper = new DatabaseHelper();
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "SELECT d_date, package_status, notes, delivery_status, payement_status FROM deliver WHERE DID = @DID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DID", DID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["d_date"].ToString();
                            textBox2.Text = reader["package_status"].ToString();
                            textBox4.Text = reader["notes"].ToString();
                            textBox5.Text = reader["delivery_status"].ToString();
                            textBox6.Text = reader["payement_status"].ToString();
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string d_date = textBox1.Text;
            string package_status = textBox2.Text;
            string notes = textBox4.Text;
            string delivery_status = textBox5.Text;
            string payment_status = textBox6.Text;

            DatabaseHelper dbHelper = new DatabaseHelper();
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();

                string query = "UPDATE deliver " +
                               "SET d_date = @d_date, " +
                               "package_status = @package_status, " +
                               "notes = @notes, " +
                               "delivery_status = @delivery_status, " +
                               "payement_status = @payment_status " +
                               "WHERE DID = @DID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@DID", DID);
                    command.Parameters.AddWithValue("@d_date", d_date);
                    command.Parameters.AddWithValue("@package_status", package_status);
                    command.Parameters.AddWithValue("@notes", notes);
                    command.Parameters.AddWithValue("@delivery_status", delivery_status);
                    command.Parameters.AddWithValue("@payment_status", payment_status);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Delivery details updated successfully.");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update delivery details.");
                    }
                }
            }
        }
    }
}
