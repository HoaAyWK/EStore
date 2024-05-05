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
    [Migration("20240505104617_Add_ShortDescription")]
    partial class Add_ShortDescription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EStore.Domain.BrandAggregate.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BrandId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Brands", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.CartAggregate.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CartId");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("Id");

                    b.ToTable("Carts", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CategoryId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ParentCategoryId");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ParentId");

                    b.HasIndex("Slug");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.CustomerAggregate.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CustomerId");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id");

                    b.HasIndex("PhoneNumber");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.DiscountAggregate.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DiscountId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DiscountPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UsePercentage")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Discounts", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.NotificationAggregate.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("NotificationId");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("From")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("From");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("To")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("To");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OrderId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("OrderNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.ProductAggregate.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductId");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductBrandId");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductCategoryId");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DiscountId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductDiscountId");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("HasVariant")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("EStore.Infrastructure.Identity.AccountToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TokenType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AccountTokens");
                });

            modelBuilder.Entity("EStore.Infrastructure.OrderSequenceManager.OrderSequence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("LastOrderNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("OrderSequences");
                });

            modelBuilder.Entity("EStore.Infrastructure.Persistence.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("EStore.Domain.CartAggregate.Cart", b =>
                {
                    b.OwnsMany("EStore.Domain.CartAggregate.Entities.CartItem", "Items", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("CartItemId");

                            b1.Property<Guid>("CartId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid?>("ProductVariantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("decimal(18, 2)");

                            b1.HasKey("Id", "CartId");

                            b1.HasIndex("CartId");

                            b1.ToTable("CartItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CartId");
                        });

                    b.Navigation("Items");
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.HasOne("EStore.Domain.CategoryAggregate.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("EStore.Domain.CustomerAggregate.Customer", b =>
                {
                    b.OwnsMany("EStore.Domain.CustomerAggregate.Entities.Address", "Addresses", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("AddressId");

                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<int>("CountryId")
                                .HasColumnType("int");

                            b1.Property<bool>("IsDefault")
                                .HasColumnType("bit");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)");

                            b1.Property<string>("ReceiverName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<int>("StateId")
                                .HasColumnType("int");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.HasKey("Id", "CustomerId");

                            b1.HasIndex("CustomerId");

                            b1.ToTable("CustomerAddresses", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("EStore.Domain.OrderAggregate.Order", b =>
                {
                    b.OwnsMany("EStore.Domain.OrderAggregate.Entities.OrderItem", "OrderItems", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("OrderItemId");

                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("DiscountAmount")
                                .HasColumnType("decimal(18, 2)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("decimal(18, 2)");

                            b1.HasKey("Id", "OrderId");

                            b1.HasIndex("OrderId");

                            b1.ToTable("OrderItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.OwnsOne("EStore.Domain.OrderAggregate.ValueObjects.ItemOrdered", "ItemOrdered", b2 =>
                                {
                                    b2.Property<Guid>("OrderItemId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("OrderItemOrderId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("ProductAttributes")
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<Guid>("ProductId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("ProductImage")
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("ProductName")
                                        .IsRequired()
                                        .HasMaxLength(200)
                                        .HasColumnType("nvarchar(200)");

                                    b2.Property<Guid?>("ProductVariantId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.HasKey("OrderItemId", "OrderItemOrderId");

                                    b2.ToTable("OrderItems");

                                    b2.WithOwner()
                                        .HasForeignKey("OrderItemId", "OrderItemOrderId");
                                });

                            b1.Navigation("ItemOrdered")
                                .IsRequired();
                        });

                    b.OwnsMany("EStore.Domain.OrderAggregate.Entities.OrderStatusHistoryTracking", "OrderStatusHistoryTrackings", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("OrderStatusHistoryTrackingId");

                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("CreatedDateTime")
                                .HasColumnType("datetime2");

                            b1.Property<int>("Status")
                                .HasColumnType("int");

                            b1.HasKey("Id", "OrderId");

                            b1.HasIndex("OrderId");

                            b1.ToTable("OrderStatusHistoryTrackings", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("EStore.Domain.OrderAggregate.ValueObjects.ShippingAddress", "ShippingAddress", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(15)
                                .HasColumnType("nvarchar(15)");

                            b1.Property<string>("ReceiverName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("nvarchar(20)");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("OrderItems");

                    b.Navigation("OrderStatusHistoryTrackings");

                    b.Navigation("ShippingAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("EStore.Domain.ProductAggregate.Product", b =>
                {
                    b.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductAttribute", "ProductAttributes", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductAttributeId");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("CanCombine")
                                .HasColumnType("bit");

                            b1.Property<bool>("Colorable")
                                .HasColumnType("bit");

                            b1.Property<int>("DisplayOrder")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.HasKey("Id", "ProductId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("ProductAttributes", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductAttributeValue", "ProductAttributeValues", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("ProductAttributeValueId");

                                    b2.Property<Guid>("ProductAttributeId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ProductId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Color")
                                        .HasMaxLength(30)
                                        .HasColumnType("nvarchar(30)");

                                    b2.Property<int>("DisplayOrder")
                                        .HasColumnType("int");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("nvarchar(100)");

                                    b2.Property<decimal>("PriceAdjustment")
                                        .HasColumnType("decimal(18, 2)");

                                    b2.Property<string>("RawCombinedAttributes")
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("Id", "ProductAttributeId", "ProductId");

                                    b2.HasIndex("ProductAttributeId", "ProductId");

                                    b2.ToTable("ProductAttributeValues", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ProductAttributeId", "ProductId");
                                });

                            b1.Navigation("ProductAttributeValues");
                        });

                    b.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductImage", "Images", b1 =>
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

                    b.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductReview", "ProductReviews", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductReviewId");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("nvarchar(2000)");

                            b1.Property<DateTime>("CreatedDateTime")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("OwnerId")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductReviewOwnerId");

                            b1.Property<int>("Rating")
                                .HasColumnType("int");

                            b1.Property<string>("RawAttributes")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("UpdatedDateTime")
                                .HasColumnType("datetime2");

                            b1.HasKey("Id", "ProductId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("ProductReviews", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductReviewComment", "ReviewComments", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("ProductReviewCommentId");

                                    b2.Property<Guid>("ProductReviewId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ProductId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Content")
                                        .IsRequired()
                                        .HasMaxLength(1)
                                        .HasColumnType("nvarchar(1)");

                                    b2.Property<DateTime>("CreatedDateTime")
                                        .HasColumnType("datetime2");

                                    b2.Property<Guid>("OwnerId")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("ProductReviewCommentOwnerId");

                                    b2.Property<Guid?>("ParentId")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("ProductReviewCommentParentId");

                                    b2.Property<DateTime>("UpdatedDateTime")
                                        .HasColumnType("datetime2");

                                    b2.HasKey("Id", "ProductReviewId", "ProductId");

                                    b2.HasIndex("ProductReviewId", "ProductId");

                                    b2.ToTable("ProductReviewComments", (string)null);

                                    b2.WithOwner()
                                        .HasForeignKey("ProductReviewId", "ProductId");
                                });

                            b1.Navigation("ReviewComments");
                        });

                    b.OwnsMany("EStore.Domain.ProductAggregate.Entities.ProductVariant", "ProductVariants", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("ProductVariantId");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AssignedProductImageIds")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("IsActive")
                                .HasColumnType("bit");

                            b1.Property<decimal?>("Price")
                                .HasColumnType("decimal(18, 2)");

                            b1.Property<string>("RawAttributeSelection")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("RawAttributes")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("StockQuantity")
                                .HasColumnType("int");

                            b1.HasKey("Id", "ProductId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("ProductVariants", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.OwnsOne("EStore.Domain.ProductAggregate.ValueObjects.AverageRating", "AverageRating", b2 =>
                                {
                                    b2.Property<Guid>("ProductVariantId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("ProductVariantProductId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("NumRatings")
                                        .HasColumnType("int");

                                    b2.Property<double>("Value")
                                        .HasColumnType("float");

                                    b2.HasKey("ProductVariantId", "ProductVariantProductId");

                                    b2.ToTable("ProductVariants");

                                    b2.WithOwner()
                                        .HasForeignKey("ProductVariantId", "ProductVariantProductId");
                                });

                            b1.Navigation("AverageRating")
                                .IsRequired();
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

                    b.Navigation("Images");

                    b.Navigation("ProductAttributes");

                    b.Navigation("ProductReviews");

                    b.Navigation("ProductVariants");
                });

            modelBuilder.Entity("EStore.Domain.CategoryAggregate.Category", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
