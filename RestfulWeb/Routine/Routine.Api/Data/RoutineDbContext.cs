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
                    Introduction="great1"
                },
                new Company()
                {
                    Id = Guid.Parse("111eadee-098d-3d3d-bece-44ddaaaee511"),
                    Name = "ms",
                    Introduction = "great2"
                },
                new Company()
                {
                    Id = Guid.Parse("222eadee-098d-3d3d-bece-44ddaaaee511"),
                    Name = "alibaba",
                    Introduction = "great3"
                }
                );

        }
    }
}
