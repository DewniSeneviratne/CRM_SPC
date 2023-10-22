using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spc
{
    public partial class Prescriptions : Form
    {
        private DatabaseHelper dbHelper;
        public Prescriptions()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void button1_Click(object sender, EventArgs e)
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

      
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Prescriptions p = new Prescriptions();
            p.Show();
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

       

        private void button9_Click(object sender, EventArgs e)
        {
            label3.Show();
            label4.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            pictureBox6.Show();
            button11.Show();
            button10.Show();
            dataGridView1.Hide();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string cid = textBox1.Text;
            string pDate = textBox2.Text;

            if (pictureBox6.Image != null)
            {
                // Get the values from the text boxes
                string CID = textBox1.Text;
                string p_dateStr = textBox2.Text; // Assuming p_date is entered as a string

                // Convert the p_date string to a DateTime
                if (DateTime.TryParse(p_dateStr, out DateTime p_date))
                {
                    // Check if an image is selected
                    if (pictureBox6.Image != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Convert the selected image to a byte array
                            pictureBox6.Image.Save(ms, ImageFormat.Jpeg); // Adjust the format as needed

                            // Create a byte array to store the image data
                            byte[] imageData = ms.ToArray();

                            // Create a DatabaseHelper instance
                            DatabaseHelper dbHelper = new DatabaseHelper();

                            // Open a SQL connection
                            using (SqlConnection connection = dbHelper.CreateConnection())
                            {
                                try
                                {
                                    connection.Open();

                                    // Create an SQL command to insert data into the "prescription" table
                                    string query = "INSERT INTO prescription (CID, p_date, imageData) VALUES (@CID, @p_date, @imageData); SELECT SCOPE_IDENTITY()";
                                    SqlCommand command = new SqlCommand(query, connection);

                                    // Add parameters
                                    command.Parameters.AddWithValue("@CID", CID);
                                    command.Parameters.AddWithValue("@p_date", p_date); // Use the parsed DateTime
                                    command.Parameters.AddWithValue("@imageData", imageData);

                                    // Execute the SQL command and retrieve the newly generated PID
                                    int newlyGeneratedPID = Convert.ToInt32(command.ExecuteScalar());

                                    if (newlyGeneratedPID > 0)
                                    {
                                        // Successfully inserted the prescription, open the AddPrescriptionDetailsForm
                                        AddPrescriptionDetailsForm addDetailsForm = new AddPrescriptionDetailsForm(newlyGeneratedPID);
                                        addDetailsForm.ShowDialog();

                                        // Clear the form after successful insertion
                                        textBox1.Clear();
                                        textBox2.Clear();
                                        pictureBox6.Image = null;
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
                    }
                    else
                    {
                        MessageBox.Show("Please select an image.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid date format. Please enter a valid date.");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file's path
                string selectedImagePath = openFileDialog1.FileName;
                

                // Display the selected image in the PictureBox
                pictureBox6.Image = Image.FromFile(selectedImagePath);

                // You can use this path to do further processing, like displaying the image or saving it.
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            
            label3.Hide();
            label4.Hide();
            label2.Hide();
            textBox1.Hide();
            textBox2.Hide();
            pictureBox6.Hide();
            button11.Hide();
            button10.Hide();
            dataGridView1.Show();
            LoadPrescriptionData();
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

        private void LoadPrescriptionData()
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "SELECT PID, CID, p_date FROM prescription";

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["ViewMedicine"].Index)
            {
                // Get the PID associated with the clicked row
                int pid = (int)dataGridView1.Rows[e.RowIndex].Cells["PID"].Value;

                
                string medicineDetails = GetMedicineDetailsForPID(pid);
                MessageBox.Show("Medicine Details for PID " + pid + ":\n\n" + medicineDetails);
            }
        }

        private void Prescriptions_Load(object sender, EventArgs e)
        {
            label3.Show();
            label4.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            pictureBox6.Show();
            button11.Show();
            button10.Show();
            dataGridView1.Hide();


        }

        private string GetMedicineDetailsForPID(int pid)
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "SELECT PM.quantity, PM.instruction, M.name " +
                               "FROM prescription_med PM " +
                               "INNER JOIN medicine M ON PM.MID = M.MID " +
                               "WHERE PM.PID = @pid";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@pid", pid);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder medicineDetails = new StringBuilder();

                        while (reader.Read())
                        {
                            int quantity = (int)reader["Quantity"];
                            string instructions = reader["Instruction"].ToString();
                            string medicineName = reader["Name"].ToString();

                            medicineDetails.AppendLine("Medicine Name: " + medicineName);
                            medicineDetails.AppendLine("Quantity: " + quantity.ToString());
                            medicineDetails.AppendLine("Instructions: " + instructions);
                            medicineDetails.AppendLine();
                        }

                        return medicineDetails.ToString();
                    }
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
