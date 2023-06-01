﻿// <auto-generated />
using System;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    [DbContext(typeof(EStoreDbContext))]
    [Migration("20230531073855_Add_ProductAttribute")]
    partial class Add_ProductAttribute
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EStore.Domain.BrandAggregate.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BrandId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CategoryId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EStore.Domain.ProductAggregate.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductId");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<decimal?>("SpecialPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("SpecialPriceEndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SpecialPriceStartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("EStore.Domain.ProductAttributeAggregate.ProductAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductAttributeId");

                    b.Property<string>("Alias")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ProductAttributes", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.HasOne("EStore.Domain.CategoryAggregate.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("EStore.Domain.ProductAggregate.Product", b =>
                {
                    b.HasOne("EStore.Domain.BrandAggregate.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EStore.Domain.CategoryAggregate.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("EStore.Domain.ProductAggregate.ProductImage", "Images", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductImageId");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("DisplayOrder")
                                .HasColumnType("int");

                            b1.Property<string>("ImageUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("IsMain")
                                .HasColumnType("bit");

                            b1.HasKey("Id", "ProductId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("ProductImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("EStore.Domain.ProductAggregate.ValueObjects.AverageRating", "AverageRating", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("NumRatings")
                                .HasColumnType("int");

                            b1.Property<double>("Value")
                                .HasColumnType("float");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("AverageRating")
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Category");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("EStore.Domain.ProductAttributeAggregate.ProductAttribute", b =>
                {
                    b.OwnsMany("EStore.Domain.ProductAttributeAggregate.ProductAttributeOptionSet", "ProductAttributeOptionSets", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductAttributeOptionSetId");

                            b1.Property<Guid>("ProductAttributeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.HasKey("Id", "ProductAttributeId");

                            b1.HasIndex("ProductAttributeId");

                            b1.ToTable("ProductAttributeOptionSets", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductAttributeId");

                            b1.OwnsMany("EStore.Domain.ProductAttributeAggregate.ProductAttributeOption", "ProductAttributeOptions", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("ProductAttributeOptionId");

                                    b2.Property<Guid>("ProductAttributeOptionSetId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ProductAttributeId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Alias")
                                        .HasMaxLength(30)
                                        .HasColumnType("nvarchar(30)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("nvarchar(100)");

                                    b2.Property<decimal>("PriceAdjustment")
                                        .HasColumnType("decimal(18,2)");

                                    b2.HasKey("Id", "ProductAttributeOptionSetId", "ProductAttributeId");

                                    b2.HasIndex("ProductAttributeOptionSetId", "ProductAttributeId");

                                    b2.ToTable("ProductAttributeOptions", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ProductAttributeOptionSetId", "ProductAttributeId");
                                });

                            b1.Navigation("ProductAttributeOptions");
                        });

                    b.Navigation("ProductAttributeOptionSets");
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
