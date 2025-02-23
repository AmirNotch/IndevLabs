using IndevLabs.Models.db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndevLabs.Models.DbConfigurations;

public class WineConfiguration : IEntityTypeConfiguration<Wine>
{
    public void Configure(EntityTypeBuilder<Wine> entity)
    {
        entity.ToTable("wines");
        
        entity.HasKey(e => e.Id)
            .HasName("wines_pkey");

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        entity.Property(e => e.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasColumnType("TEXT"); // Используем TEXT вместо VARCHAR

        entity.Property(e => e.Year)
            .HasColumnName("year")
            .IsRequired();

        entity.Property(e => e.Brand)
            .HasColumnName("brand")
            .IsRequired()
            .HasColumnType("TEXT"); // Используем TEXT вместо VARCHAR

        entity.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasColumnType("TEXT"); // Используем TEXT вместо VARCHAR
    }
}