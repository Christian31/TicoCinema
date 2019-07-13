using System.Configuration;

namespace TicoCinema.WebApplication.Utils
{
    public static class WebConfigHelper
    {
        public static string SqlConnectionString => GetSqlConnectionString();

        private static string GetSqlConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
    }
}
