using System;

namespace WMS.Model
{
    public class InboundOrderDetail
    {
        public int Id { get; set; }
        public int InboundOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal ExpectedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } // Pending, Received, Completed
        public string Remarks { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        // Navigation properties
        public InboundOrder InboundOrder { get; set; }
        public Product Product { get; set; }
    }
} 