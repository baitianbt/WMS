using System;

namespace WMS.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal UnitPrice { get; set; }
        public string Unit { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
} 