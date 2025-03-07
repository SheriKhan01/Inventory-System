using IMS.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.Name).IsUnique();

            builder.Property(r => r.Description)
                   .IsRequired(false); // Allow NULL values for Description
        }
    }
}
