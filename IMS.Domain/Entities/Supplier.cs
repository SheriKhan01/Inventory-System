using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities
{
    public class Supplier:BaseEntity
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }

        // Navigation Property
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
