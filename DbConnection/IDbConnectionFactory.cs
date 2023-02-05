using Microsoft.Data.SqlClient;

namespace BlogApp.DbConnection
{
    public interface IDbConnectionFactory
    {
        public SqlConnection GetDefaultConnection();
    }
}
