using System.Data;

namespace Reusables.Repositories.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
