using System;
using System.Collections.Generic;

namespace WMS.Model
{
    public class InboundOrder
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int WarehouseId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContact { get; set; }
        public string SupplierPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedArrivalDate { get; set; }
        public DateTime? ActualArrivalDate { get; set; }
        public string Status { get; set; } // Pending, Received, Completed, Cancelled
        public string Remarks { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        // Navigation properties
        public Warehouse Warehouse { get; set; }
        public List<InboundOrderDetail> Details { get; set; }
    }
} 