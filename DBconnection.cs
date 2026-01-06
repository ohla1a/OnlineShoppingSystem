using System.Data.SqlClient;

namespace OnlineShoppingManagementSystem
{
    internal static class DBConnection
    {
        // change Data Source if you use .\SQLEXPRESS
        public static string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineShoppingDB;Integrated Security=True;Connect Timeout=30";


        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
