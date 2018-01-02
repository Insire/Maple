using System.Configuration;

namespace Maple.Data
{
    public class ConnectionStringManager : IConnectionStringManager
    {
        public string Get()
        {
            var setting = ConfigurationManager.ConnectionStrings["Main"];

            return setting.ConnectionString;
        }
    }
}
