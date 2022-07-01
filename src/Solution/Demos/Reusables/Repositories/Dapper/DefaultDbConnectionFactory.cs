using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Reusables.Repositories.Dapper
{
    public class DefaultDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        public DefaultDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            // TODO cache connectionString ?
            var connectionString = _configuration.GetConnectionString("AdventureWorks");
            if (string.IsNullOrEmpty(connectionString))
                throw new ApplicationException("Could not fetch DB connection from config");

            return new SqlConnection(connectionString);
        }
    }
}
