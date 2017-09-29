using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UISWeb.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFile_CustomerRequest_CustomerRequestLabNo",
                table: "CustomerFile");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerRequestLabNo",
                table: "CustomerFile",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFile_CustomerRequest_CustomerRequestLabNo",
                table: "CustomerFile",
                column: "CustomerRequestLabNo",
                principalTable: "CustomerRequest",
                principalColumn: "LabNo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFile_CustomerRequest_CustomerRequestLabNo",
                table: "CustomerFile");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerRequestLabNo",
                table: "CustomerFile",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFile_CustomerRequest_CustomerRequestLabNo",
                table: "CustomerFile",
                column: "CustomerRequestLabNo",
                principalTable: "CustomerRequest",
                principalColumn: "LabNo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
