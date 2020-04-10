﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Routine.Api.Data;

namespace Routine.Api.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    [Migration("20200410152732_addemplpyeedata")]
    partial class addemplpyeedata
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("4b0a7ba5-cd18-4ac8-9ddf-f64ce88f3899"),
                            CompanyId = new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G001",
                            FirstName = "googlemary",
                            LastName = "king",
                            gender = 2
                        },
                        new
                        {
                            Id = new Guid("58debf79-56be-4742-a0dc-62c821d00176"),
                            CompanyId = new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G002",
                            FirstName = "googledave",
                            LastName = "nick",
                            gender = 1
                        },
                        new
                        {
                            Id = new Guid("2bc70937-c9b3-4fcd-856a-e9c0485b2cfb"),
                            CompanyId = new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "M001",
                            FirstName = "msmary",
                            LastName = "king",
                            gender = 2
                        },
                        new
                        {
                            Id = new Guid("ac8780e0-8276-4aaf-b258-402aa915b9a0"),
                            CompanyId = new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "M002",
                            FirstName = "msdave",
                            LastName = "nick",
                            gender = 1
                        },
                        new
                        {
                            Id = new Guid("a4b11e0c-1c0f-436e-b4aa-ca1340eae0f5"),
                            CompanyId = new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A001",
                            FirstName = "alibabamary",
                            LastName = "king",
                            gender = 2
                        },
                        new
                        {
                            Id = new Guid("04847cdf-e8ae-4073-8d01-71a9b89b0b88"),
                            CompanyId = new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"),
                            DateofBirth = new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A001",
                            FirstName = "alibabadave",
                            LastName = "nick",
                            gender = 1
                        });
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