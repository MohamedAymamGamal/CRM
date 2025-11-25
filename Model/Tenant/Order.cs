using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Model.Tenant
{
    public class Order
    {

        public int Id { get; set; }  // Primary key
        public int CustomerId { get; set; } // Optional, if you have a customer table
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}