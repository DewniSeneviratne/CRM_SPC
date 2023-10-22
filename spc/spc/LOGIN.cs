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
    public partial class LOGIN : Form
    {
        private DatabaseHelper dbHelper;
        public LOGIN()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string un = textBox1.Text;
            string pw = textBox2.Text;

            
            bool isValidUser = CheckCredentials(un, pw);

            if (isValidUser)
            {
                home obj = new home();
                this.Hide();
                obj.Show();
            }
            else
            {
                MessageBox.Show("Incorrect Credentials!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public bool CheckCredentials(string username, string password)
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();

                
                using (SqlCommand command = new SqlCommand("spAuthenticateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;    
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int authenticationResult = (int)reader["AuthenticationResult"];
                            return authenticationResult == 1;
                        }
                    }
                }
            }

            return false;
        }

    }
}
