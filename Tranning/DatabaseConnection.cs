using Microsoft.Data.SqlClient;

namespace Tranning
{
    public class DatabaseConnection
    {
        public static string GetStrConnection()
        {
            string strConnection = @"Data Source=DUK-FUC;Initial Catalog=Tranning;User ID=sa;Password=20072003;TrustServerCertificate=True;";
            return strConnection;
        }

        public static SqlConnection GetSqlConnection()
        {
            string strConnection = DatabaseConnection.GetStrConnection();
            SqlConnection connection = new SqlConnection(strConnection);
            return connection;
        }
    }
}
