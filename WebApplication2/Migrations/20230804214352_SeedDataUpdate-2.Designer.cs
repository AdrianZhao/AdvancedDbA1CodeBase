﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication2.Data;

#nullable disable

namespace WebApplication2.Migrations
{
    [DbContext(typeof(RefactorContext))]
    [Migration("20230804214352_SeedDataUpdate-2")]
    partial class SeedDataUpdate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApplication2.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.Property<Guid>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Number");

                    b.HasIndex("BrandId");

                    b.ToTable("Laptops");
                });

            modelBuilder.Entity("WebApplication2.Models.LaptopQuantity", b =>
                {
                    b.Property<Guid>("LaptopNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StoreLocationNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("LaptopNumber", "StoreLocationNumber");

                    b.HasIndex("StoreLocationNumber");

                    b.ToTable("LaptopQuantities");
                });

            modelBuilder.Entity("WebApplication2.Models.StoreLocation", b =>
                {
                    b.Property<Guid>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number");

                    b.ToTable("StoreLocations");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.HasOne("WebApplication2.Models.Brand", "Brand")
                        .WithMany("Laptops")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("WebApplication2.Models.LaptopQuantity", b =>
                {
                    b.HasOne("WebApplication2.Models.Laptop", "Laptop")
                        .WithMany("LaptopQuantities")
                        .HasForeignKey("LaptopNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication2.Models.StoreLocation", "StoreLocation")
                        .WithMany("LaptopQuantities")
                        .HasForeignKey("StoreLocationNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laptop");

                    b.Navigation("StoreLocation");
                });

            modelBuilder.Entity("WebApplication2.Models.Brand", b =>
                {
                    b.Navigation("Laptops");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.Navigation("LaptopQuantities");
                });

            modelBuilder.Entity("WebApplication2.Models.StoreLocation", b =>
                {
                    b.Navigation("LaptopQuantities");
                });
#pragma warning restore 612, 618
        }
    }
}
