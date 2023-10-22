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
    public partial class EditMedicineForm : Form
    {
        private DatabaseHelper dbHelper;
        private int MID;

        public EditMedicineForm(int medicineID, string medicineName, int quantity, DateTime expDate, string description)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            MID = medicineID;
            MedicineID = medicineID.ToString();

            MedicineName = medicineName;
            Quantity = quantity.ToString();
            ExpDate = expDate.ToString(); 
            Description = description;
        }

      
        public string MedicineID
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }
        public string MedicineName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string Quantity
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string ExpDate
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value; }
        }

        public string Description
        {
            get { return textBox5.Text; }
            set { textBox5.Text = value; }
        }

        
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

            string updatedName = textBox1.Text;
            if (!int.TryParse(textBox2.Text, out int updatedQuantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            if (!DateTime.TryParse(textBox4.Text, out DateTime updatedExpDate))
            {
                MessageBox.Show("Please enter a valid expiration date.");
                return;
            }

            string updatedDescription = textBox5.Text;


            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "UPDATE medicine SET name = @name, quantity = @quantity, exp_date = @expDate, desciption = @description WHERE MID = @medicineID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", updatedName);
                command.Parameters.AddWithValue("@quantity", updatedQuantity);
                command.Parameters.AddWithValue("@expDate", updatedExpDate);
                command.Parameters.AddWithValue("@description", updatedDescription);
                command.Parameters.AddWithValue("@medicineID", MID); // Set the unique identifier of the medicine record

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update data.");
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
   
}
