using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace QuanLyKho.API.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
