using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Infrastructure.Data.Configurations;

internal class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("ServiceRequests");


        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        // CustomerId
        builder.Property(x => x.CustomerId)
            .IsRequired()
            .HasMaxLength(450);

        // ProviderId
        builder.Property(x => x.ProviderId)
            .HasMaxLength(450);

        // Location
        builder.Property(x => x.Latitude)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(x => x.Longitude)
            .IsRequired()
            .HasPrecision(9, 6);

        // Timestamps
        builder.Property(x => x.CreatedAt)
            .IsRequired();


        // Relationships (مهم جدًا)
        //builder.HasOne<User>()
        //    .WithMany()
        //    .HasForeignKey(x => x.CustomerId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.HasOne<User>()
        //    .WithMany()
        //    .HasForeignKey(x => x.ProviderId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}
