using System;

namespace WMS.Model
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public string Location { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        // Navigation properties
        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
} 