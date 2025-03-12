using System;
using System.Collections.Generic;

namespace WMS.Model
{
    public class OutboundOrder
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int WarehouseId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedShippingDate { get; set; }
        public DateTime? ActualShippingDate { get; set; }
        public string Status { get; set; } // Pending, Picked, Shipped, Completed, Cancelled
        public string Remarks { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        // Navigation properties
        public Warehouse Warehouse { get; set; }
        public List<OutboundOrderDetail> Details { get; set; }
    }
} 