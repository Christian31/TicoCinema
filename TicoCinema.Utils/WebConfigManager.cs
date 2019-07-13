using System.Configuration;

namespace TicoCinema.Utils
{
    public static class WebConfigManager
    {
        public static string SqlConnectionString => GetSqlConnectionString();

        private static string GetSqlConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
    }
}
