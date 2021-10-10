﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bike_selling_app.Infrastructure.Persistence;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
{
    [DbContext(typeof(ApplicationDbContextMySql))]
    partial class ApplicationDbContextMySqlModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CapitalItemProject", b =>
                {
                    b.Property<int>("CapitalItemsId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.HasKey("CapitalItemsId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("CapitalItemProject");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("double");

                    b.Property<string>("PurchasedFrom")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SerialNumber", "Make", "Model")
                        .IsUnique();

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ItemType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasDiscriminator<string>("ItemType").HasValue("Item");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateEnded")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateStarted")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<double>("NetValue")
                        .HasColumnType("double");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.CapitalItem", b =>
                {
                    b.HasBaseType("bike_selling_app.Domain.Entities.Item");

                    b.Property<double>("Cost")
                        .HasColumnType("double");

                    b.Property<int>("UsageCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("capital");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.ExpenseItem", b =>
                {
                    b.HasBaseType("bike_selling_app.Domain.Entities.Item");

                    b.Property<int?>("CapitalItemId")
                        .HasColumnType("int");

                    b.Property<int?>("NonCapitalItemId")
                        .HasColumnType("int");

                    b.Property<int?>("RevenueItemId")
                        .HasColumnType("int");

                    b.Property<double>("UnitCost")
                        .HasColumnType("double");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasIndex("CapitalItemId");

                    b.HasIndex("NonCapitalItemId");

                    b.HasIndex("RevenueItemId");

                    b.HasIndex("Name", "DatePurchased")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("expense");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.NonCapitalItem", b =>
                {
                    b.HasBaseType("bike_selling_app.Domain.Entities.Item");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<double>("UnitCost")
                        .HasColumnType("double")
                        .HasColumnName("NonCapitalItem_UnitCost");

                    b.Property<int>("Units")
                        .HasColumnType("int")
                        .HasColumnName("NonCapitalItem_Units");

                    b.HasIndex("ProjectId");

                    b.HasDiscriminator().HasValue("non-capital");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.RevenueItem", b =>
                {
                    b.HasBaseType("bike_selling_app.Domain.Entities.Item");

                    b.Property<DateTime>("DateSold")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsPending")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PlatformSoldOn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("RevenueItem_ProjectId");

                    b.Property<string>("RevenueItemType")
                        .HasColumnType("longtext");

                    b.Property<double>("SalePrice")
                        .HasColumnType("double");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasIndex("ProjectId");

                    b.HasDiscriminator().HasValue("revenue");
                });

            modelBuilder.Entity("CapitalItemProject", b =>
                {
                    b.HasOne("bike_selling_app.Domain.Entities.CapitalItem", null)
                        .WithMany()
                        .HasForeignKey("CapitalItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bike_selling_app.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Bike", b =>
                {
                    b.HasOne("bike_selling_app.Domain.Entities.Project", "Project")
                        .WithMany("Bikes")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.ExpenseItem", b =>
                {
                    b.HasOne("bike_selling_app.Domain.Entities.CapitalItem", null)
                        .WithMany("ExpenseItems")
                        .HasForeignKey("CapitalItemId");

                    b.HasOne("bike_selling_app.Domain.Entities.NonCapitalItem", null)
                        .WithMany("ExpenseItems")
                        .HasForeignKey("NonCapitalItemId");

                    b.HasOne("bike_selling_app.Domain.Entities.RevenueItem", null)
                        .WithMany("ExpenseItems")
                        .HasForeignKey("RevenueItemId");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.NonCapitalItem", b =>
                {
                    b.HasOne("bike_selling_app.Domain.Entities.Project", "Project")
                        .WithMany("NonCapitalItems")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.RevenueItem", b =>
                {
                    b.HasOne("bike_selling_app.Domain.Entities.Project", null)
                        .WithMany("RevenueItems")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Project", b =>
                {
                    b.Navigation("Bikes");

                    b.Navigation("NonCapitalItems");

                    b.Navigation("RevenueItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.CapitalItem", b =>
                {
                    b.Navigation("ExpenseItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.NonCapitalItem", b =>
                {
                    b.Navigation("ExpenseItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.RevenueItem", b =>
                {
                    b.Navigation("ExpenseItems");
                });
#pragma warning restore 612, 618
        }
    }
}
