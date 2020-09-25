using System.Data.SqlClient;

namespace WebApplication1.Helpers
{
    public class DBSQLServerUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string connString = @"Server=OR160558\SQLEXPRESS;Database=ad_interface;Integrated Security=true;";

            return new SqlConnection(connString);
        }
    }
}
