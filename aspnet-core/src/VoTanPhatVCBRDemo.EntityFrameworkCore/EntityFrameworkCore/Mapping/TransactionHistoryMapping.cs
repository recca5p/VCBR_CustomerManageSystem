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
    public class TransactionHistoryMapping : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.HasKey(th => th.Id);
            builder.HasOne(th => th.Customer)
                .WithMany(c => c.TransactionHistories)
                .HasForeignKey(c => c.CustomerId);
        }
    }
}
