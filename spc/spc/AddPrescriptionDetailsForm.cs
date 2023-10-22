using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace spc
{
    public partial class AddPrescriptionDetailsForm : Form
    {
        private DatabaseHelper dbHelper;

        public AddPrescriptionDetailsForm(int newlyGeneratedPID)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            textBox1.Text = newlyGeneratedPID.ToString();
        }

      

        private void LoadMedicineNames()
        {
            // Fetch medicine names from the database
            List<string> medicineNames = GetMedicineNamesFromDatabase();

            // Populate the ComboBox with medicine names
            comboBox1.DataSource = medicineNames;
        }

        private List<string> GetMedicineNamesFromDatabase()
        {
            List<string> medicineNames = new List<string>();

            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "SELECT name FROM medicine";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medicineNames.Add(reader["name"].ToString());
                    }
                }
            }

            return medicineNames;
        }

    

        private int GetMedicineIDFromDatabase(string medicineName)
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "SELECT MID FROM medicine WHERE name = @medicineName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@medicineName", medicineName);
                    return (int)command.ExecuteScalar();
                }
            }
        }

        private void InsertPrescriptionMedDetails(int PID, int MedicineID, int quantity, string instructions)
        {
            using (SqlConnection connection = dbHelper.CreateConnection())
            {
                connection.Open();
                string query = "INSERT INTO prescription_med (PID, MID, quantity, instruction) VALUES (@PID, @MedicineID, @quantity, @instructions)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PID", PID);
                    command.Parameters.AddWithValue("@MedicineID", MedicineID);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@instructions", instructions);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int PID = int.Parse(textBox1.Text);
            string selectedMedicineName = comboBox1.SelectedItem.ToString();
            int MedicineID = GetMedicineIDFromDatabase(selectedMedicineName);
            int quantity = int.Parse(textBox2.Text);
            string instructions = textBox3.Text;

            // Insert the details into the prescription_med table
            InsertPrescriptionMedDetails(PID, MedicineID, quantity, instructions);

            textBox2.Clear();
            textBox3.Clear();
            
            
        }

        private void AddPrescriptionDetailsForm_Load_1(object sender, EventArgs e)
        {
            LoadMedicineNames();
        }
    }
}
