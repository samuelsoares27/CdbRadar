using System.Data;

namespace CdbRadar.Repository.Abstractions
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
