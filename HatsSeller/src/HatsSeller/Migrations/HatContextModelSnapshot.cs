using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HatsSeller.Data;

namespace HatsSeller.Migrations
{
    [DbContext(typeof(HatContext))]
    partial class HatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HatsSeller.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("CategoryID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("HatsSeller.Models.Hat", b =>
                {
                    b.Property<int>("HatID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("Description");

                    b.Property<string>("HatName");

                    b.Property<string>("PathOfFile");

                    b.Property<decimal>("Price");

                    b.Property<int>("SupplierID");

                    b.HasKey("HatID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("SupplierID");

                    b.ToTable("Hat");
                });

            modelBuilder.Entity("HatsSeller.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("HomePhone");

                    b.Property<int>("MobilePhone");

                    b.Property<string>("SupplierName");

                    b.Property<int>("WorkPhone");

                    b.HasKey("SupplierID");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("HatsSeller.Models.Hat", b =>
                {
                    b.HasOne("HatsSeller.Models.Category", "Category")
                        .WithMany("Hats")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HatsSeller.Models.Supplier", "Supplier")
                        .WithMany("Hats")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
