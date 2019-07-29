using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace TicoCinema.WebApplication.Utils
{
    public static class RazorHelper
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from Enum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = GetEnumDescription(e) };
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && 
                attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static string GetRestrictionFormat(this int restriction)
        {
            return restriction == 0 ? "Sin restricción" : string.Format("Mayores de {0} años", restriction);
        }

        public static string GetCategoriesSelectedFormat(this IList<SelectListItem> categories)
        {
            var selectedCategories = categories.Where(item => item.Selected).ToList();
            return selectedCategories.Count() == 0 ? "Sin categorias" : string.Join(" - ", selectedCategories.Select(item => item.Text).ToList());
        }
    }
}