//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicoCinema.WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleDetails
    {
        public long SaleDetailId { get; set; }
        public long SaleId { get; set; }
        public int ProductType { get; set; }
        public long ProductId { get; set; }
        public long ProductHistoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    
        public virtual Sale Sale { get; set; }
    }
}
