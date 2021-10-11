﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bike_selling_app.Infrastructure.Persistence;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    [DbContext(typeof(ApplicationDbContextSqlite))]
    partial class ApplicationDbContextSqliteModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CapitalItemProject", b =>
                {
                    b.Property<int>("CapitalItemsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CapitalItemsId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("CapitalItemProject");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("REAL");

                    b.Property<string>("PurchasedFrom")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SerialNumber", "Make", "Model")
                        .IsUnique();

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.CapitalItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Cost")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("UsageCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CapitalItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.ExpenseItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CapitalItemId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("NonCapitalItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RevenueItemId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitCost")
                        .HasColumnType("REAL");

                    b.Property<int>("Units")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CapitalItemId");

                    b.HasIndex("NonCapitalItemId");

                    b.HasIndex("Name", "DatePurchased")
                        .IsUnique();

                    b.ToTable("ExpenseItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.NonCapitalItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitCost")
                        .HasColumnType("REAL");

                    b.Property<int>("UnitsPurchased")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UnitsRemaining")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("NonCapitalItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DateEnded")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateStarted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<double>("NetValue")
                        .HasColumnType("REAL");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.RevenueItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSold")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPending")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("PlatformSoldOn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RevenueItemType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<double>("SalePrice")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("RevenueItems");
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
                        .HasForeignKey("CapitalItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("bike_selling_app.Domain.Entities.NonCapitalItem", null)
                        .WithMany("ExpenseItems")
                        .HasForeignKey("NonCapitalItemId")
                        .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity("bike_selling_app.Domain.Entities.CapitalItem", b =>
                {
                    b.Navigation("ExpenseItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.NonCapitalItem", b =>
                {
                    b.Navigation("ExpenseItems");
                });

            modelBuilder.Entity("bike_selling_app.Domain.Entities.Project", b =>
                {
                    b.Navigation("Bikes");

                    b.Navigation("NonCapitalItems");

                    b.Navigation("RevenueItems");
                });
#pragma warning restore 612, 618
        }
    }
}
