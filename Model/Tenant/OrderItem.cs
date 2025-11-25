using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Model.Tenant
{
    public class OrderItem
    {
        public int Id { get; set; }  // Primary key
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        // Navigation properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}