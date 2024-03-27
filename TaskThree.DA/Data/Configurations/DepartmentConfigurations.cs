using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.DA.Models;

namespace TaskThree.DA.Data.Configurations
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id  ).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).IsRequired().HasMaxLength(10).HasColumnType("varchar");
            builder.Property(D => D.Code).IsRequired().HasMaxLength(10).HasColumnType("varchar");
            builder.HasMany(D  => D.Employees).WithOne(E => E.Department).HasForeignKey(E => E.DepartmentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
