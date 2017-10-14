using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UISWeb.Migrations
{
    public partial class MigrateRequest20171014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "CustomerRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoadedFID",
                table: "CustomerRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoadedMS",
                table: "CustomerRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "CustomerRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "CustomerRequest");

            migrationBuilder.DropColumn(
                name: "LoadedFID",
                table: "CustomerRequest");

            migrationBuilder.DropColumn(
                name: "LoadedMS",
                table: "CustomerRequest");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "CustomerRequest");
        }
    }
}
