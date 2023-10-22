using System.Data.SqlClient;

namespace spc
{
    internal class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            connectionString = "Server=DEWNI\\SQLEXPRESS;Database=CRMKotteDB;Integrated Security=True;";
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}