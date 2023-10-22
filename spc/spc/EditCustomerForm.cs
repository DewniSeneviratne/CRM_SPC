using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace spc
{
    public partial class EditCustomerForm : Form
    {
        private DatabaseHelper dbHelper;
        public EditCustomerForm()
        {
            InitializeComponent();

            dbHelper = new DatabaseHelper();
        }

        public string CID
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }

        public string Name
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string TelNum
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string Address
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value; }
        }

        public string Email
        {
            get { return textBox5.Text; }
            set { textBox5.Text = value; }
        }

        private void button10_Click(object sender, EventArgs e)
        {
       
    string updatedCID = textBox3.Text;
            string updatedName = textBox1.Text;
            string updatedTelNum = textBox2.Text;
            string updatedAddress = textBox4.Text;
            string updatedEmail = textBox5.Text;

            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "UPDATE customer SET name = @name, tel_num = @telNum WHERE CID = @cid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", updatedName);
                command.Parameters.AddWithValue("@telNum", updatedTelNum);
                command.Parameters.AddWithValue("@cid", updatedCID);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
     
                    if (!string.IsNullOrEmpty(updatedAddress) && !string.IsNullOrEmpty(updatedEmail))
                    {
                        query = "UPDATE loyal_customer SET address = @address, email = @email WHERE CID = @cid";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@address", updatedAddress);
                        command.Parameters.AddWithValue("@email", updatedEmail);
                        command.Parameters.AddWithValue("@cid", updatedCID);
                        rowsAffected = command.ExecuteNonQuery();
                    }

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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
