using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Configurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.ToTable("InventoryItems");

            builder.HasKey(i => i.Id);

            // Index for faster retrieval
            builder.HasIndex(i => i.Name);

            // Relationships
            builder.HasOne(i => i.Category)
                .WithMany(c => c.InventoryItems)
                .HasForeignKey(i => i.CategoryId);

            builder.HasOne(i => i.Supplier)
                .WithMany(s => s.InventoryItems)
                .HasForeignKey(i => i.SupplierId);
        }
    }
}
