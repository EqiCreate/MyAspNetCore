using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class addproperycompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Employees_Companies_CompanyId",
            //    table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Companies",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Companies",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "Companies",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("111eadee-098d-3d3d-bece-44ddaaaee511"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Software", "SF" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("222eadee-098d-3d3d-bece-44ddaaaee511"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "China", "Shopping", "SF" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("dddeadee-098d-3d3d-bece-44ddaaaee511"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "WEB", "WEB" });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Employees_Companies_CompanyId",
            //    table: "Employees",
            //    column: "CompanyId",
            //    principalTable: "Companies",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Companies");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
