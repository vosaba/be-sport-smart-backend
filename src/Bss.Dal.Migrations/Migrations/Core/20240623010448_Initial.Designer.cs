﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Bss.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bss.Dal.Migrations.Migrations.Core
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20240623010448_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bss.Component.Core.Models.Computation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<int>("Engine")
                        .HasColumnType("integer");

                    b.Property<string>("Formula")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<string>>("_requiredComputations")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("RequiredComputations");

                    b.Property<List<string>>("_requiredMeasures")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("RequiredMeasures");

                    b.HasKey("Id");

                    b.HasIndex("Engine");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Type");

                    b.ToTable("Computations");
                });

            modelBuilder.Entity("Bss.Component.Core.Models.Measure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<int>("InputSource")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Options")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Type");

                    b.ToTable("Measures");
                });
#pragma warning restore 612, 618
        }
    }
}
