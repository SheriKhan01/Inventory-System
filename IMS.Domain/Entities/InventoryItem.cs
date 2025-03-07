using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities
{
    public class InventoryItem:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign Keys
        public Guid CategoryId { get; set; }
        public Guid SupplierId { get; set; }

        // Navigation Properties
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
    }
}
