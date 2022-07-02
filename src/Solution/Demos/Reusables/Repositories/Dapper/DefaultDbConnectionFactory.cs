using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Reusables.Repositories.Dapper
{
    public class DefaultDbConnectionFactory : IDbConnectionFactory
    {
        private const int ConnectionPoolMinSize = 100;

        private readonly IConfiguration _configuration;
        
        private string _connectionString = null;
        public DefaultDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            string connectionString = null;
            if (_connectionString == null)
            {
                lock(this)
                {
                    if (_connectionString != null)
                    {
                        connectionString = _connectionString;
                    }
                    else
                    {
                        _connectionString = connectionString = GetConnectionString();
                    }
                }
            }
            else
            {
                connectionString = _connectionString;
            }



            return new SqlConnection(connectionString);
        }

        private string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("AdventureWorks");
            if (string.IsNullOrEmpty(connectionString))
                throw new ApplicationException("Could not fetch DB connection from config");

            if (!connectionString.EndsWith(";"))
                connectionString += ";";

            connectionString += $"Min Pool Size={ConnectionPoolMinSize};";

            return connectionString;
        }
    }
}
