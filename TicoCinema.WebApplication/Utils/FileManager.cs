using System;
using System.IO;
using System.Web;

namespace TicoCinema.WebApplication.Utils
{
    public static class FileManager
    {
        private static string GetFileName(string name, string uploadedFile)
        {
            string extension = Path.GetExtension(uploadedFile);
            string fileName = name + DateTime.Now.ToString("yyMMddHHmmssff");

            return fileName + extension;
        }

        public static string SaveFoodImage(string name, HttpPostedFileBase image)
        {
            string fileName = GetFileName(name, image.FileName);
            string foodImagesFolder = HttpContext.Current.Server.MapPath(WebConfigHelper.FoodImagesPath);

            string filePath = Path.Combine(foodImagesFolder, fileName);
            image.SaveAs(filePath);

            return fileName;
        }

        public static string ReplaceFoodImage(string name, HttpPostedFileBase image, string oldImagePath)
        {
            string foodImagesFolder = HttpContext.Current.Server.MapPath(WebConfigHelper.FoodImagesPath);

            string filePath = Path.Combine(foodImagesFolder, oldImagePath);
            File.Delete(filePath);

            return SaveFoodImage(name, image);
        }

        public static string GetFoodImagePath(string imagePath)
        {
            return Path.Combine(WebConfigHelper.FoodImagesPath, imagePath);
        }

        public static string SaveMovieImage(string name, HttpPostedFileBase image)
        {
            string fileName = GetFileName(name, image.FileName);
            string foodImagesFolder = HttpContext.Current.Server.MapPath(WebConfigHelper.MovieImagesPath);

            string filePath = Path.Combine(foodImagesFolder, fileName);
            image.SaveAs(filePath);

            return fileName;
        }

        public static string GetMovieImagePath(string imagePath)
        {
            return Path.Combine(WebConfigHelper.MovieImagesPath, imagePath);
        }
    }
}