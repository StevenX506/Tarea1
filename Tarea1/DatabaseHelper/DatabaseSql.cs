using Microsoft.Data.SqlClient;
using System.Data;

namespace Tarea1.DatabaseHelper
{
    public static class DatabaseSql
    {
        private static string connectionString = "Server=DESKTOP-BS9VBDJ\\SQLEXPRESS;Database=AdventureWorks2025;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable executeStoredProcedure(string sp)
        {
            using SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            using SqlCommand cmd = new SqlCommand(sp, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            adapter.Fill(dt);

            return dt;
        }
    }
}