using System.Collections.Generic;

namespace TicoCinema.WebApplication.ViewModels
{
    public class Province
    {
        public int IdProvince { get; set; }
        public string ProvinceName { get; set; }
        public List<Canton> Cantons { get; set; }
    }

    public class Canton
    {
        public int IdCanton { get; set; }
        public string CantonName { get; set; }
        public List<District> Districts { get; set; }
    }

    public class District
    {
        public int IdDistrict { get; set; }
        public string DistrictName { get; set; }
    }
}