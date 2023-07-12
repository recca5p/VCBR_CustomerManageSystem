using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoTanPhatVCBRDemo.Domain;

namespace VoTanPhatVCBRDemo.EntityFrameworkCore.Mapping
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.TransactionHistories)
                .WithOne(th => th.Customer)
                .HasForeignKey(c => c.Id);
            builder.HasOne<IdentityUser>().WithMany().HasForeignKey(c => c.UserId);
        }
    }
}
