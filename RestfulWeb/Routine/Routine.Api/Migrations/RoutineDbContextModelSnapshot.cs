﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Routine.Api.Data;

namespace Routine.Api.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    partial class RoutineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Routine.Api.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"),
                            Introduction = "great1",
                            Name = "google"
                        },
                        new
                        {
                            Id = new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"),
                            Introduction = "great2",
                            Name = "ms"
                        },
                        new
                        {
                            Id = new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"),
                            Introduction = "great3",
                            Name = "alibaba"
                        });
                });

            modelBuilder.Entity("Routine.Api.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("gender")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Routine.Api.Entities.Employee", b =>
                {
                    b.HasOne("Routine.Api.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}