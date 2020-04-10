using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class addemplpyeedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("4b0a7ba5-cd18-4ac8-9ddf-f64ce88f3899"), new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "G001", "googlemary", "king", 2 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("58debf79-56be-4742-a0dc-62c821d00176"), new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "G002", "googledave", "nick", 1 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("2bc70937-c9b3-4fcd-856a-e9c0485b2cfb"), new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "M001", "msmary", "king", 2 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("ac8780e0-8276-4aaf-b258-402aa915b9a0"), new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "M002", "msdave", "nick", 1 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("a4b11e0c-1c0f-436e-b4aa-ca1340eae0f5"), new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1999, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "A001", "alibabamary", "king", 2 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateofBirth", "EmployeeNo", "FirstName", "LastName", "gender" },
                values: new object[] { new Guid("04847cdf-e8ae-4073-8d01-71a9b89b0b88"), new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"), new DateTime(1991, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "A001", "alibabadave", "nick", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("04847cdf-e8ae-4073-8d01-71a9b89b0b88"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("2bc70937-c9b3-4fcd-856a-e9c0485b2cfb"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("4b0a7ba5-cd18-4ac8-9ddf-f64ce88f3899"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("58debf79-56be-4742-a0dc-62c821d00176"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a4b11e0c-1c0f-436e-b4aa-ca1340eae0f5"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("ac8780e0-8276-4aaf-b258-402aa915b9a0"));
        }
    }
}
