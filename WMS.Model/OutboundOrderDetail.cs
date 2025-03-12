using System;

namespace WMS.Model
{
    public class OutboundOrderDetail
    {
        public int Id { get; set; }
        public int OutboundOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } // Pending, Picked, Shipped, Completed
        public string Remarks { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        // Navigation properties
        public OutboundOrder OutboundOrder { get; set; }
        public Product Product { get; set; }
    }
} 