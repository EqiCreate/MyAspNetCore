using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Data
{
    public class RoutineDbContext:DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext>options):base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().HasOne(x => x.Company).WithMany(x => x.Employees).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>().HasData(
                new Company()
                { 
                    Id=Guid.Parse("dddeadee-098d-3d3d-bece-44ddaaaee511"),
                    Name="google",
                    Introduction="great1",
                },
                new Company()
                {
                    Id = Guid.Parse("111eadee-098d-3d3d-bece-44ddaaaee511"),
                    Name = "ms",
                    Introduction = "great2",
                },
                new Company()
                {
                    Id = Guid.Parse("222eadee-098d-3d3d-bece-44ddaaaee511"),
                    Name = "alibaba",
                    Introduction = "great3",
                }
                );
            modelBuilder.Entity<Employee>().HasData(
                    new Employee() { DateofBirth = new DateTime(1999, 11, 3), EmployeeNo = "G001", FirstName = "googlemary", LastName = "king", gender = Gender.女,CompanyId=Guid.Parse("dddeadee-098d-3d3d-bece-44ddaaaee511"),Id=Guid.Parse("4b0a7ba5-cd18-4ac8-9ddf-f64ce88f3899") },
                    new Employee() { DateofBirth = new DateTime(1991, 11, 3), EmployeeNo = "G002", FirstName = "googledave", LastName = "nick", gender = Gender.男,CompanyId= Guid.Parse("dddeadee-098d-3d3d-bece-44ddaaaee511"), Id = Guid.Parse("58debf79-56be-4742-a0dc-62c821d00176") },

                    new Employee() { DateofBirth = new DateTime(1999, 11, 3), EmployeeNo = "M001", FirstName = "msmary", LastName = "king", gender = Gender.女, CompanyId = Guid.Parse("111eadee-098d-3d3d-bece-44ddaaaee511"), Id = Guid.Parse("2bc70937-c9b3-4fcd-856a-e9c0485b2cfb") },
                    new Employee() { DateofBirth = new DateTime(1991, 11, 3), EmployeeNo = "M002", FirstName = "msdave", LastName = "nick", gender = Gender.男, CompanyId = Guid.Parse("111eadee-098d-3d3d-bece-44ddaaaee511"), Id = Guid.Parse("ac8780e0-8276-4aaf-b258-402aa915b9a0") },

                    new Employee() { DateofBirth = new DateTime(1999, 11, 3), EmployeeNo = "A001", FirstName = "alibabamary", LastName = "king", gender = Gender.女, CompanyId = Guid.Parse("222eadee-098d-3d3d-bece-44ddaaaee511"), Id = Guid.Parse("a4b11e0c-1c0f-436e-b4aa-ca1340eae0f5") },
                    new Employee() { DateofBirth = new DateTime(1991, 11, 3), EmployeeNo = "A001", FirstName = "alibabadave", LastName = "nick", gender = Gender.男, CompanyId = Guid.Parse("222eadee-098d-3d3d-bece-44ddaaaee511"), Id = Guid.Parse("04847cdf-e8ae-4073-8d01-71a9b89b0b88") }
                    );

        }
    }
}
