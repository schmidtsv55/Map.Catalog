using System;
using System.Collections.Generic;
using Map.Catalog.Api.MapDB.Models;
using Microsoft.EntityFrameworkCore;

namespace Map.Catalog.Api.MapDB;

public partial class MapdbContext : DbContext
{
    public MapdbContext(DbContextOptions<MapdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MapHeatingSystem> MapHeatingSystems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MapHeatingSystem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("map_heating_system");

            entity.Property(e => e.DestinationMap)
                .HasMaxLength(255)
                .HasColumnName("destination_map");
            entity.Property(e => e.DestinationPath)
                .HasMaxLength(255)
                .HasColumnName("destination_path");
            entity.Property(e => e.SourceMap)
                .HasMaxLength(255)
                .HasColumnName("source_map");
            entity.Property(e => e.SourcePath)
                .HasMaxLength(255)
                .HasColumnName("source_path");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
