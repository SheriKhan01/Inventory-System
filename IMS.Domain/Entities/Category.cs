using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation Property (no initialization)
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
