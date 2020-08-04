
using MySqlConnector;

namespace SyaApi.Plugins
{
    public static class DatabaseConnector
    {
        private static string ConnectionString { get; set; }

        static DatabaseConnector()
        {
            DatabaseConnector.ConnectionString = ConfigReader.GetConfig("ConnectionString");
        }

        public static MySqlConnection Connect()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
