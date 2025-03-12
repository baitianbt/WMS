using System;
using System.Data;
using System.Data.SqlClient;

namespace WMS.DAL
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {


            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
} 