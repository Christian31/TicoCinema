using System.Configuration;

namespace TicoCinema.WebApplication.Utils
{
    public static class WebConfigHelper
    {
        public static string SqlConnectionString => GetSqlConnectionString();
        public static string DefaultAdminPassword => GetDefaultAdminPassword();
        public static string FoodImagesPath => GetFoodImagesPath();
        public static string MovieImagesPath => GetMovieImagesPath();

        private static string GetMovieImagesPath()
        {
            return ConfigurationManager.AppSettings["movieImagesPath"];
        }

        private static string GetFoodImagesPath()
        {
            return ConfigurationManager.AppSettings["foodImagesPath"];
        }

        private static string GetSqlConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        
        private static string GetDefaultAdminPassword()
        {
            return ConfigurationManager.AppSettings["defaultAdminPassword"];
        }

    }
}
